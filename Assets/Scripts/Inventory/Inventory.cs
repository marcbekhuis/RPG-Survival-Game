using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected new string name;
    [SerializeField] protected Vector2Int size;
    [SerializeField] protected Inventory linkedInventory;
    [SerializeField] protected ItemAmount[] startingItems;

    protected Item[,] items;

    public delegate void InventorySetup();
    public event InventorySetup onInventorySetup = delegate { };

    public delegate void ItemSlotChange(Vector2Int index, Item item);
    public event ItemSlotChange onItemSlotChange = delegate { };

    public Vector2Int Size
    {
        get => size;
    }

    public virtual void FullSetup()
    {
        Setup();
        AddStartItems();
    }

    public void Setup()
    {
        items = new Item[size.x, size.y];
        onInventorySetup();
    }

    public void AddStartItems()
    {
        foreach (var itemAmount in startingItems)
        {
            AddItem(itemAmount.item, itemAmount.GetAmount());
        }
    }

    // GetItem : get item by index
    public Item GetItem(Vector2Int index)
    {
        return items[index.x, index.y];
    }

    // GetItem : get item by index
    public Item GetItem(int x, int y)
    {
        return items[x, y];
    }

    // AddItem : add item 
    public bool AddItem(Item item)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (!items[x, y])
                {
                    items[x, y] = item;
                    item.inventory = this;
                    onItemSlotChange(new Vector2Int(x,y), items[x, y]);
                    return false;
                }
            }
        }
        if (linkedInventory) return linkedInventory.AddItem(item);
        else return false;
    }

    // AddItem : add item using item template, specify amount
    public int AddItem(Item item, int amount, bool stack = true)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (items[x, y])
                {
                    if (!stack) continue;
                    if (items[x, y].name == item.name)
                    {
                        if (!items[x, y].StackFull)
                        {
                            int spaceAvailable = items[x, y].stackSize - items[x, y].amount;
                            items[x, y].amount += amount;
                            amount -= spaceAvailable;
                            if (amount <= 0) return 0;
                        }
                    }
                }
                else
                {
                    if (amount >= item.stackSize)
                    {
                        amount -= item.stackSize;
                        items[x, y] = item.Create(item.stackSize, this);
                        onItemSlotChange(new Vector2Int(x, y), items[x, y]);
                    }
                    else
                    {
                        items[x, y] = item.Create(amount, this);
                        amount = 0;
                        onItemSlotChange(new Vector2Int(x, y), items[x, y]);
                    }
                    if (amount <= 0) return 0;
                }
            }
        }
        if (linkedInventory) return linkedInventory.AddItem(item, amount, stack);
        else return amount;
    }

    // AddItem : Add item to specified index
    public void AddItem(Item item, Vector2Int index)
    {
        items[index.x, index.y] = item;
        item.inventory = this;
        onItemSlotChange(index, items[index.x, index.y]);
    }

    // AddItem : Add item template to specified index with amount
    public void AddItem(Item item, int amount, Vector2Int index)
    {
        items[index.x, index.y] = item.Create(amount, this);
        onItemSlotChange(index, items[index.x, index.y]);
    }

    // RemoveItem : Remove item
    public bool RemoveItem(Item item)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (items[x, y])
                {
                    if (items[x, y] == item)
                    {
                        items[x, y] = null;
                        onItemSlotChange(new Vector2Int(x, y), null);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // RemoveItem : Remove item at index
    public void RemoveItem(Vector2Int index)
    {
        items[index.x, index.y] = null;
        onItemSlotChange(index, null);
        return;
    }

    // HasItem : returns true if item template is in inventory
    public bool HasItem(Item item)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (items[x, y])
                {
                    if (items[x, y] == item)
                    {
                        return true;
                    }
                }
            }
        }
        if (linkedInventory) return linkedInventory.HasItem(item);
        else return false;
    }

    // GetItemAmount : returns int amount, item template is in inventory
    public int GetItemAmount(Item item)
    {
        int amount = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (items[x, y])
                {
                    if (items[x, y].name == item.name)
                    {
                        amount += items[x, y].amount;
                    }
                }
            }
        }
        if (linkedInventory) amount += linkedInventory.GetItemAmount(item);
        return amount;
    }

    // HasEmptySlot : returns true if inventory has empty slot
    public bool HasEmptySlot()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (!items[x, y])
                {
                    return true;
                }
            }
        }
        if (linkedInventory) return linkedInventory.HasEmptySlot();
        else return false;
    }

    // SlotEmpty : returns true if slot at given index is empty
    public bool SlotEmpty(Vector2Int index)
    {
        return !items[index.x, index.y];
    }

    // HasSpaceForItem : returns amount of item that fits in inventory
    public int HasSpaceForItem(Item item)
    {
        int amount = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (items[x, y])
                {
                    if (items[x, y].name == item.name)
                    {
                        amount += items[x, y].stackSize - items[x, y].amount;
                    }
                }
                else amount += item.stackSize;
            }
        }

        if (linkedInventory) amount += linkedInventory.HasSpaceForItem(item);
        return amount;
    }

    // GetEmptySlot : Get first empty slot in the inventory
    public Vector2Int GetEmptySlot()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (!items[x, y])
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return default;
    }

    // SplitItem : Split item
    public bool SplitItem(Item item)
    {
        if (HasEmptySlot())
        {
            if (item.amount > 1)
            {
                float amount = item.amount / 2f;
                item.amount = Mathf.CeilToInt(amount);
                AddItem(item, Mathf.FloorToInt(amount), false);
                return true;
            }
        }
        return false;
    }

    // SplitItem : Split item index
    public bool SplitItem(Vector2Int index)
    {
        return SplitItem(GetItem(index));
    }

    //SlotIsItem : Returns true if item at index is same
    public bool SlotIsItem(Item item, Vector2Int index)
    {
        if (!SlotEmpty(index))
            return GetItem(index).name == item.name;
        return false;
    }

    //StackItem : Returns int left over amount, Adds item to slot
    public int StackItem(Item item, Vector2Int index)
    {
        Item slotItem = GetItem(index);
        int space = slotItem.stackSize - slotItem.amount;

        int amount = item.amount - space;
        slotItem.amount += item.amount;

        if (amount > 0) return AddItem(item, amount);
        else return 0;
    }

    public void Clear()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (items[x, y])
                {
                    items[x, y] = null;
                    onItemSlotChange(new Vector2Int(x,y), null);
                }
            }
        }
    }

    public void Save()
    {
        JsonSavingLoading.SaveData<SaveFile>(new SaveFile(items, size), "Inventory", name);
    }

    public void Load()
    {
        SaveFile saveFile = JsonSavingLoading.LoadData<SaveFile>("Inventory", name);
        Clear();
        foreach (var itemString in saveFile.items)
        {
            Item.SaveFile itemSave = JsonUtility.FromJson<Item.SaveFile>(itemString);
            Item item = ItemList.instance.Getitem(itemSave.itemName).LoadItem(itemString);
            AddItem(item, itemSave.index);
        }
    }

    [System.Serializable]
    public class SaveFile
    {
        public SaveFile(Item[,] items, Vector2Int size)
        {
            List<string> itemsList = new List<string>();
            for (int y = 0; y <size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (items[x, y])
                        itemsList.Add(JsonUtility.ToJson(items[x, y].GetSave(new Vector2Int(x,y))));
                }
            }
            this.items = itemsList.ToArray();
        }

        public string[] items;
    }
}

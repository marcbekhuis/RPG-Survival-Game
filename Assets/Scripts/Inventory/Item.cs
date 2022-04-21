using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item")]
    public string displayName;
    public string description;
    public Sprite icon;
    public Sprite dropSprite;
    [Min(1)] public int stackSize = 1;
    [SerializeField] private float weight = 0;
    
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public bool used = false;

    protected int amountVar;

    public delegate void AmountChange(int amount);
    public event AmountChange onAmountChange;

    public int amount 
    {
        get => amountVar; 
        set 
        {
            amountVar = Mathf.Clamp(value, 0, stackSize);
            if (amountVar <= 0)
                inventory.RemoveItem(this);
            else
                onAmountChange(amountVar);
        }
    }

    public float Weight { get => weight * amountVar; }

    public bool StackFull { get => amountVar >= stackSize; }

    public Item(Item item, int amount, Inventory inventory)
    {
        this.displayName = item.displayName;
        this.description = item.description;
        this.icon = item.icon;
        this.dropSprite = item.dropSprite;
        this.stackSize = Mathf.Clamp(item.stackSize, 1, int.MaxValue);
        this.inventory = inventory;
        this.amountVar = amount;
        this.weight = item.weight;
    }

    public Item(SaveFile saveFile, Item item)
    {
        this.displayName = saveFile.displayName;
        this.description = item.description;
        this.icon = item.icon;
        this.dropSprite = item.dropSprite;
        this.stackSize = Mathf.Clamp(saveFile.stackSize, 1, int.MaxValue);
        this.weight = saveFile.weight;
        this.amountVar = saveFile.amount;
    }

    public virtual Item Create(int amount, Inventory inventory)
    {
        Item item = new Item(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public virtual Item Create(SaveFile saveFile)
    {
        Item item = new Item(saveFile, this);
        item.name = this.name;
        return item;
    }

    public bool IsType<T>() where T : Item
    {
        try
        {
            return (T)this;
        }
        catch
        {
            return false;
        }
    }

    public bool TryGetType<T>(out T item) where T : Item
    {
        try
        {
            item = (T)this;
            return item;
        }
        catch
        {
            item = null;
            return false;
        }
    }

    public T GetType<T>() where T : Item
    {
        return (T)this;
    }

    public virtual string GetInformation()
    {
        return this.name + "\n"
            + description + "\n"
            + "Amount: " + amountVar + "/" + stackSize;
    }

    public virtual SaveFile GetSave(Vector2Int index)
    {
        return new SaveFile(this.name, displayName, stackSize, Weight, amount, index);
    }

    public virtual Item LoadItem(string item)
    {
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(item);
        return Create(saveFile);
    }

    [System.Serializable]
    public class SaveFile 
    {
        public string itemName;
        public string displayName;
        public int stackSize;
        public float weight;
        public int amount;
        public Vector2Int index;

        public SaveFile(string itemName, string displayName, int stackSize, float weight, int amount, Vector2Int index)
        {
            this.itemName = itemName;
            this.displayName = displayName;
            this.stackSize = stackSize;
            this.weight = weight;
            this.amount = amount;
            this.index = index;
        }
    }
}
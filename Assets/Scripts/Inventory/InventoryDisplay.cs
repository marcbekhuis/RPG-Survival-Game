using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] protected Inventory inventoryVar;
    [SerializeField] protected ItemContainer itemContainerPrefab;
    [SerializeField] protected Transform inventoryParent;

    protected ItemContainer[,] itemContainers;
    protected RectTransform rectTransform;

    public virtual Inventory inventory
    {
        get => inventoryVar;
        set
        {
            if (inventory)
            {
                inventory.onInventorySetup -= Setup;
                inventory.onItemSlotChange -= ChangeSlot;
            }
            inventoryVar = value;
            if (inventory)
            {
                inventory.onInventorySetup += Setup;
                inventory.onItemSlotChange += ChangeSlot;
                //Setup();
            }
        }
    }

    protected void Awake()
    {
        if (inventory)
        {
            inventory.onInventorySetup += Setup;
            inventory.onItemSlotChange += ChangeSlot;
        }
        rectTransform = this.GetComponent<RectTransform>();
    }

    public bool IsOverItem(Vector2 position, out Item item, out Vector2Int index)
    {
        for (int y = 0; y < inventoryVar.Size.y; y++)
        {
            for (int x = 0; x < inventoryVar.Size.x; x++)
            {
                if (itemContainers[x, y].IsOver(position))
                {
                    index = new Vector2Int(x,y);
                    item = itemContainers[x, y].item;
                    return true;
                }
            }
        }
        index = Vector2Int.zero;
        item = null;
        return false;
    }

    protected void Setup()
    {
        itemContainers = new ItemContainer[inventoryVar.Size.x, inventoryVar.Size.y];
        for (int y = 0; y < inventoryVar.Size.y; y++)
        {
            for (int x = 0; x < inventoryVar.Size.x; x++)
            {
                ItemContainer itemContainer = Instantiate(itemContainerPrefab, inventoryParent);
                itemContainers[x, y] = itemContainer;
                itemContainer.item = inventory.GetItem(new Vector2Int(x, y));
            }
        }
    }

    protected void ChangeSlot(Vector2Int index, Item item)
    {
        itemContainers[index.x, index.y].item = item;
    }

    public bool IsOnInventory(Vector2 position)
    {
        Vector2 halfSize = rectTransform.rect.size * 0.5f * CanvasHelper.instance.CanvasScale();
        float highPosition = rectTransform.position.y + halfSize.y;
        float lowPosition = rectTransform.position.y - halfSize.y;
        float leftPosition = rectTransform.position.x - halfSize.x;
        float rightPosition = rectTransform.position.x + halfSize.x;
        if (position.x > leftPosition && position.x < rightPosition)
        {
            if (position.y > lowPosition && position.y < highPosition)
            {
                return true;
            }
        }
        return false;
    }
}

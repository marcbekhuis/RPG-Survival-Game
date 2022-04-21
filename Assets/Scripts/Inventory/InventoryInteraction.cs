using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    [SerializeField] private InventoryDisplay[] inventoryDisplays;

    private Item item;
    private InventoryDisplay inventoryDisplay;
    private Vector2Int index;
    private bool wasSlotClick = false;

    public static Item movingItem;
    private InventoryDisplay movingInventoryDisplay;
    private Vector2Int movingIndex;

    private void Update()
    {
        GetItemMouseOver();

        if (InputManager.instance.GetKeyDown(InputActions.ItemInteraction))
        {
            InteractionMenu();
        }
        if (!ItemInteractionMenuManager.instance.IsActive)
        {
            if (InputManager.instance.GetKeyDown(InputActions.MoveItem))
            {
                MoveItem();
            }
        }
    }

    public void InteractionMenu()
    {
        if (item)
        {
            ItemInteractionMenuManager.instance.Activate(item);
        }
    }

    public void MoveItem()
    {
        if (movingItem)
        {
            if (wasSlotClick)
            {
                if (item != movingItem)
                {
                    if (inventoryDisplay.inventory.SlotEmpty(index))
                    {
                        movingInventoryDisplay.inventory.RemoveItem(movingItem);
                        inventoryDisplay.inventory.AddItem(movingItem, index);
                    }
                    else if (inventoryDisplay.inventory.SlotIsItem(movingItem, index))
                    {
                        movingInventoryDisplay.inventory.RemoveItem(movingItem);
                        int amount = inventoryDisplay.inventory.StackItem(movingItem, index);
                        if (amount > 0) movingInventoryDisplay.inventory.AddItem(movingItem, amount, movingIndex);
                    }
                    else
                    {
                        movingInventoryDisplay.inventory.RemoveItem(movingItem);
                        inventoryDisplay.inventory.RemoveItem(item);

                        movingInventoryDisplay.inventory.AddItem(item, movingIndex);
                        inventoryDisplay.inventory.AddItem(movingItem, index);
                    }
                }
                movingItem = null;
                movingInventoryDisplay = null;
                movingIndex = Vector2Int.zero;
            }
        }
        else if(!item.used)
        {
            movingItem = item;
            movingInventoryDisplay = inventoryDisplay;
            movingIndex = index;
        }
    }

    private void GetItemMouseOver()
    {
        wasSlotClick = false;
        item = null;
        index = Vector2Int.zero;
        inventoryDisplay = null;

        foreach (var inventoryDisplayLoop in inventoryDisplays)
        {
            if (inventoryDisplayLoop.gameObject.activeSelf)
            {
                if (inventoryDisplayLoop.inventory)
                {
                    if (inventoryDisplayLoop.IsOverItem(Input.mousePosition, out Item tempItem, out Vector2Int tempIndex))
                    {
                        item = tempItem;
                        index = tempIndex;
                        wasSlotClick = true;
                        inventoryDisplay = inventoryDisplayLoop;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    [SerializeField] private InventoryDisplay[] inventoryDisplays;
    [SerializeField] private MenuHandler menuHandler;
    [SerializeField] private DropItem dropItemPrefab;

    private Item item;
    private InventoryDisplay inventoryDisplay;
    private Vector2Int index;
    private bool wasSlotClick = false;

    public static Item movingItem;
    private InventoryDisplay movingInventoryDisplay;
    private Vector2Int movingIndex;

    private void Start()
    {
        menuHandler.onInventoryToggle += OnInventoryToggle;
    }

    private void Update()
    {
        GetItemMouseOver();

        if (InputManager.instance.GetKeyDown(InputActions.ItemInteraction))
        {
            InteractionMenu();
        }
        if (!ItemInteractionMenuManager.instance.IsActive)
        {
            if(movingItem)
                MovingIcon.instance.MoveIcon();
            if (InputManager.instance.GetKeyDown(InputActions.MoveItem))
            {
                MoveItem();
            }
            if (InputManager.instance.GetKeyDown(InputActions.DropItem))
            {
                DropItem();
            }
        }
    }

    public void DropItem()
    {
        if (item)
        {
            item.inventory.RemoveItem(item);
            DropItem dropItem = Instantiate(dropItemPrefab, item.inventory.transform.position, new Quaternion(0, 0, 0, 0));
            dropItem.Setup(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 5, item, fullItem: true);
        }
    }

    private void OnInventoryToggle(bool active)
    {
        if (!active)
            OnCloseInventory();
    }

    private void OnCloseInventory()
    {
        if (!movingItem)
            return;

        movingItem.inventory.AddItem(movingItem, movingIndex);
        movingItem = null;
        movingInventoryDisplay = null;
        movingIndex = Vector2Int.zero;
        MovingIcon.instance.ToggleIcon(false, null);
    }

    private void InteractionMenu()
    {
        if (item)
        {
            ItemInteractionMenuManager.instance.Activate(item);
        }
    }

    private void MoveItem()
    {
        if (movingItem)
        {
            if (wasSlotClick)
            {
                if (item != movingItem)
                {
                    if (inventoryDisplay.inventory.SlotEmpty(index))
                    {
                        //movingInventoryDisplay.inventory.RemoveItem(movingItem);
                        inventoryDisplay.inventory.AddItem(movingItem, index);
                    }
                    else if (inventoryDisplay.inventory.SlotIsItem(movingItem, index))
                    {
                        //movingInventoryDisplay.inventory.RemoveItem(movingItem);
                        int amount = inventoryDisplay.inventory.StackItem(movingItem, index);
                        if (amount > 0) movingInventoryDisplay.inventory.AddItem(movingItem, amount, movingIndex);
                    }
                    else
                    {
                        //movingInventoryDisplay.inventory.RemoveItem(movingItem);
                        //inventoryDisplay.inventory.RemoveItem(item);

                        movingInventoryDisplay.inventory.AddItem(item, movingIndex);
                        inventoryDisplay.inventory.AddItem(movingItem, index);
                    }
                }
                movingItem = null;
                movingInventoryDisplay = null;
                movingIndex = Vector2Int.zero;
                MovingIcon.instance.ToggleIcon(false, null);
            }
        }
        else if(item)
        {
            if (!item.used)
            {
                item.inventory.RemoveItem(item);
                movingItem = item;
                movingInventoryDisplay = inventoryDisplay;
                movingIndex = index;
                MovingIcon.instance.ToggleIcon(true, item.icon);
            }
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

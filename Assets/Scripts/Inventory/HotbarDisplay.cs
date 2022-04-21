using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarDisplay : InventoryDisplay
{
    [Header("Hotbar")]
    //[SerializeField] private Sprite selectBackground;
    //[SerializeField] private Sprite defaultBackground;

    [SerializeField] private Color selectBackground;
    [SerializeField] private Color defaultBackground;

    protected new void Awake()
    {
        if (inventory)
        {
            inventory.onInventorySetup += Setup;
            inventory.onItemSlotChange += ChangeSlot;
            ((HotbarInventory)inventory).onSelectedItemChanged += SelectSlot;
        }
        rectTransform = this.GetComponent<RectTransform>();
    }

    public override Inventory inventory
    {
        get => inventoryVar;
        set
        {
            if (inventory)
            {
                inventory.onInventorySetup -= Setup;
                inventory.onItemSlotChange -= ChangeSlot;
                ((HotbarInventory)inventory).onSelectedItemChanged -= SelectSlot;
            }
            inventoryVar = value;
            if (inventory)
            {
                inventory.onInventorySetup += Setup;
                inventory.onItemSlotChange += ChangeSlot;
                ((HotbarInventory)inventory).onSelectedItemChanged += SelectSlot;
                //Setup();
            }
        }
    }

    protected void SelectSlot(Vector2Int index, Vector2Int oldIndex)
    {
        //itemContainers[index.x, index.y].ChangeBackgroundImage(selectBackground);
        //itemContainers[oldIndex.x, oldIndex.y].ChangeBackgroundImage(defaultBackground);

        itemContainers[oldIndex.x, oldIndex.y].ChangeBackgroundColor(defaultBackground);
        itemContainers[index.x, index.y].ChangeBackgroundColor(selectBackground);
    }
}

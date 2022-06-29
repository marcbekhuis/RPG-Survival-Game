using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : Singleton<CraftingSystem>
{
    [SerializeField] private Transform craftingParent;
    [SerializeField] private CraftingDisplay craftingPrefab;

    private Inventory inventory;

    public void Setup(Inventory inventory)
    {
        this.inventory = inventory;

        foreach (var craftingItem in CraftingList.instance.craftingItems)
        {
            CraftingDisplay craftingDisplay = Instantiate(craftingPrefab, craftingParent);
            craftingDisplay.Setup(craftingItem, this.inventory);
            this.inventory.onInventoryChange += craftingDisplay.UpdateDisplay;
        }
    }
}

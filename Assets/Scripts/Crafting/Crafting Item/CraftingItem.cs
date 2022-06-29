using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Item", menuName = "Items/Crafting Item")]
public class CraftingItem : ScriptableObject
{
    public Sprite icon;
    public ItemAmount[] requiredItems;
    public ItemAmount[] resultItems;

    public bool CanCraft(Inventory inventory)
    {
        foreach (var item in requiredItems)
        {
            if (inventory.HasItem(item.item, inventory))
            {
                if (inventory.GetItemAmount(item.item, inventory) < item.GetAmount())
                    return false;
            }
            else
                return false;
        }
        return true;
    }

    public void Craft(Inventory inventory)
    {
        foreach (var itemAmount in requiredItems)
        {
            int amount = itemAmount.GetAmount();

            while (amount > 0)
            {
                Item item = inventory.GetItem(itemAmount.item, inventory);
                int tempAmount = item.amount;
                item.amount -= amount;
                amount -= tempAmount;
            }
        }

        foreach (var itemAmount in resultItems)
        {
            inventory.AddItem(itemAmount.item, itemAmount.GetAmount(), inventory);
        }
    }
}

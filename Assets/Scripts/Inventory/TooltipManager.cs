using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private Tooltip tooltip;
    private List<ItemDisplay> itemDisplays = new List<ItemDisplay>();

    public void Activate(Item item)
    {
        tooltip.informationText.text = item.GetInformation();
        tooltip.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void AddItemdisplay(ItemDisplay itemDisplay)
    {
        itemDisplays.Add(itemDisplay);
    }

    public void RemoveItemdisplay(ItemDisplay itemDisplay)
    {
        itemDisplays.Remove(itemDisplay);
    }

    public bool HasItemdisplay(ItemDisplay itemDisplay)
    {
        return itemDisplays.Contains(itemDisplay);
    }

    private void Update()
    {
        if(!ItemInteractionMenuManager.instance.IsActive && InventoryInteraction.movingItem == null)
        {
            foreach (var itemDisplay in itemDisplays)
            {
                if (itemDisplay.IsOver(Input.mousePosition))
                {
                    Activate(itemDisplay.item);
                    return;
                }
            }
        }
        Deactivate();
    }
}

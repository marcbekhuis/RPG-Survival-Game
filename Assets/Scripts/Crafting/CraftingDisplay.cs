using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingDisplay : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image highlight;
    [SerializeField] private Button button;

    private CraftingItem craftingItem;

    public void Setup(CraftingItem craftingItem, Inventory inventory)
    {
        this.craftingItem = craftingItem;
        itemIcon.sprite = this.craftingItem.icon;
        button.onClick.AddListener(() => craftingItem.Craft(inventory));
        UpdateDisplay(inventory);
    }

    public void UpdateDisplay(Inventory inventory)
    {
        if (craftingItem.CanCraft(inventory))
        {
            highlight.color = Color.green;
            button.interactable = true;
        }
        else
        {
            highlight.color = Color.red;
            button.interactable = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionMenuManager : Singleton<ItemInteractionMenuManager>
{
    [SerializeField] private ItemInteractionMenu interactionMenu;

    public GameObject user;
    public bool IsActive { get => interactionMenu.gameObject.activeSelf; }

    public void Activate(Item item)
    {   
        interactionMenu.ChangeAnchor();
        interactionMenu.transform.position = Input.mousePosition;
        interactionMenu.gameObject.SetActive(true);
        interactionMenu.splitButton.onClick.RemoveAllListeners();
        interactionMenu.splitButton.onClick.AddListener(() => item.inventory.SplitItem(item));
        interactionMenu.splitButton.onClick.AddListener(Deactivate);

        if(item.TryGetType<UsableItem>(out UsableItem usableItem))
        {
            interactionMenu.useButton.gameObject.SetActive(true);
            interactionMenu.useButton.onClick.RemoveAllListeners();
            interactionMenu.useButton.onClick.AddListener(() => usableItem.UseItem(user));
            interactionMenu.useButton.onClick.AddListener(Deactivate);
        }
        else
        {
            interactionMenu.useButton.gameObject.SetActive(false);
        }
    }

    public void Deactivate()
    {
        interactionMenu.gameObject.SetActive(false);
    }
}

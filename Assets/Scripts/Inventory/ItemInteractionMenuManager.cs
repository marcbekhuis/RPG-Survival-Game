using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionMenuManager : Singleton<ItemInteractionMenuManager>
{
    [SerializeField] private ItemInteractionMenu interactionMenu;
    [SerializeField] private DropItem dropItemPrefab;

    [HideInInspector] public GameObject user;
    [HideInInspector] public CharacterMovement characterMovement;
    public bool IsActive { get => interactionMenu.gameObject.activeSelf; }

    public void Activate(Item item)
    {   
        interactionMenu.ChangeAnchor();
        interactionMenu.transform.position = Input.mousePosition;
        interactionMenu.gameObject.SetActive(true);
        interactionMenu.splitButton.onClick.RemoveAllListeners();
        interactionMenu.splitButton.onClick.AddListener(() => item.inventory.SplitItem(item));
        interactionMenu.splitButton.onClick.AddListener(Deactivate);
        interactionMenu.dropButton.onClick.RemoveAllListeners();
        interactionMenu.dropButton.onClick.AddListener(()=> DropItem(item));
        interactionMenu.dropButton.onClick.AddListener(Deactivate);

        if (item.TryGetType<UsableItem>(out UsableItem usableItem))
        {
            interactionMenu.useButton.gameObject.SetActive(true);
            interactionMenu.useButton.onClick.RemoveAllListeners();
            interactionMenu.useButton.onClick.AddListener(() => usableItem.UseItem(user, characterMovement.lastDirection));
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

    public void DropItem(Item item)
    {
        if (item)
        {
            DropItem dropItem = Instantiate(dropItemPrefab, item.inventory.transform.position, new Quaternion(0, 0, 0, 0));
            dropItem.Setup(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 5, item, fullItem: true);
            item.inventory.RemoveItem(item);
        }
    }
}

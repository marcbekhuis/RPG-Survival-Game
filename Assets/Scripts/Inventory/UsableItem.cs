using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UseableItem", menuName = "Items/UsableItem")]
public class UsableItem : Item
{
    [Header("Usable Item")]
    public InputActions action;

    public UsableItem(UsableItem item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
        action = item.action;
    }

    public UsableItem(SaveFile saveFile, UsableItem item) : base(saveFile, item)
    {
        action = item.action;
    }

    public override Item Create(int amount, Inventory inventory)
    {
        UsableItem item = new UsableItem(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public override Item Create(SaveFile saveFile)
    {
        UsableItem item = new UsableItem(saveFile, this);
        item.name = this.name;
        return item;
    }

    public void UseItemAction(GameObject user)
    {
        if (InputManager.instance.GetKeyDown(action))
            UseItem(user);
    }

    public virtual void UseItem(GameObject user)
    {
    }

    public virtual void OnSelect(GameObject user)
    {
    }
}

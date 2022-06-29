using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UseableItem", menuName = "Items/UsableItem")]
public class UsableItem : Item
{
    [Header("Usable Item")]
    [Min(0)] public float cooldown;
    public InputActions action;

    [HideInInspector] public float cooldownTimer;

    public delegate void DisplayCooldown();
    public event DisplayCooldown onDisplayCooldown = delegate { };

    public UsableItem(UsableItem item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
        action = item.action;
        this.cooldown = item.cooldown;
        cooldownTimer = GameManager.instance.time;
    }

    public UsableItem(SaveFile saveFile, UsableItem item) : base(saveFile, item)
    {
        action = item.action;
        this.cooldown = item.cooldown;
        cooldownTimer = GameManager.instance.time;
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

    public void UseItemAction(GameObject user, Vector2 direction)
    {
        if (InputManager.instance.GetKeyDown(action))
            UseItem(user, direction);
    }

    public void TriggerCooldownDisplay()
    {
        onDisplayCooldown();
    }

    public virtual void UseItem(GameObject user, Vector2 direction)
    {
    }

    public virtual void OnSelect(GameObject user, Vector2 direction)
    {
    }

    public virtual void OnDeselect(GameObject user, Vector2 direction)
    {
    }
}

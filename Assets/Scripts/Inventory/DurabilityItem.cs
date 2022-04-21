using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new DurabilityItem", menuName = "Items/Durability Item")]
public class DurabilityItem : UsableItem
{
    [Header("Durability")]
    [Min(0)] public float maxDurability = 0;

    protected float durabilityVar = 0;

    public delegate void DurabilityChanged(float durability, float maxDurability);
    public event DurabilityChanged onDurabilityChanged;

    public DurabilityItem(DurabilityItem item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
        this.maxDurability = item.maxDurability;
        this.durabilityVar = item.maxDurability;
    }

    public DurabilityItem(SaveFile saveFile, DurabilityItem item) : base(saveFile, item)
    {
        this.maxDurability = item.maxDurability;
        //this.durabilityVar = saveFile.durabilityVar;
    }

    public override Item Create(int amount, Inventory inventory)
    {
        DurabilityItem item = new DurabilityItem(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public override Item Create(SaveFile saveFile)
    {
        DurabilityItem item = new DurabilityItem(saveFile, this);
        item.name = this.name;
        return item;
    }

    public float durability
    {
        get => durabilityVar;
        set
        {
            durabilityVar = Mathf.Clamp(value, 0 , maxDurability);
            if (durability <= 0) amount--;
            onDurabilityChanged(durabilityVar, maxDurability);
        }
    }

    public override void UseItem(GameObject user)
    {
        durability--;
    }

    public override string GetInformation()
    {
        return this.name + "\n"
            + description + "\n"
            + "Amount: " + amountVar + "/" + stackSize + "\n"
            + "Durability: " + durabilityVar + "/" + maxDurability;
    }
}

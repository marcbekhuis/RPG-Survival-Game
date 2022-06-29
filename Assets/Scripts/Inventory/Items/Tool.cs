using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Items/Tool")]
public class Tool : Weapon
{
    public Tool(SaveFile saveFile, Weapon item) : base(saveFile, item)
    {
    }

    public Tool(Weapon item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
    }

    public override Item Create(int amount, Inventory inventory)
    {
        Tool item = new Tool(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public override Item Create(SaveFile saveFile)
    {
        Tool item = new Tool(saveFile, this);
        item.name = this.name;
        return item;
    }

    public override IEnumerator UseWeapon(GameObject user, Vector2 direction)
    {
        used = true;
        if(damageDelay > 0)
            yield return new WaitForSeconds(damageDelay);
        foreach (var target in GetTargets(user, direction))
        {
            if (target.TryGetComponent<Resource>(out Resource resource))
            {
                resource.Hit(damage);
                if (singleHit) break;
            }
        }
        durability--;
        if(duration - damageDelay > 0)
            yield return new WaitForSeconds(duration - damageDelay);
        used = false;
    }
}

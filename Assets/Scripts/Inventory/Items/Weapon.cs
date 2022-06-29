using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class Weapon : DurabilityItem
{
    [Header("Weapon")]
    public float damage;
    public float angle;
    [Min(0)] public float radius;
    [Min(0)] public float duration;
    public bool singleHit;
    public float damageDelay;
    public LayerMask targetLayer;

    public Weapon(SaveFile saveFile, Weapon item) : base(saveFile, item)
    {
        this.damage = item.damage;
        this.angle = item.angle;
        this.radius = item.radius;
        this.duration = Mathf.Clamp(item.duration, item.damageDelay, int.MaxValue);
        this.damageDelay = item.damageDelay;
        this.targetLayer = item.targetLayer;
    }

    public Weapon(Weapon item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
        this.damage = item.damage;
        this.angle = item.angle;
        this.radius = item.radius;
        this.duration = Mathf.Clamp(item.duration, item.damageDelay, int.MaxValue);
        this.damageDelay = item.damageDelay;
        this.targetLayer = item.targetLayer;
    }

    public override Item Create(int amount, Inventory inventory)
    {
        Weapon item = new Weapon(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public override Item Create(SaveFile saveFile)
    {
        Weapon item = new Weapon(saveFile, this);
        item.name = this.name;
        return item;
    }

    public override string GetInformation()
    {
        return "<b>" + this.name + "</b>\n"
            + description + "\n\n"
            + "Damage: " + damage + "\n"
            + "Duration: " + duration + "\n"
            + "Cooldown: " + cooldown + "\n"
            + "Amount: " + amountVar + "/" + stackSize + "\n"
            + "Durability: " + durabilityVar + "/" + maxDurability;
    }

    public override void UseItem(GameObject user, Vector2 direction)
    {
        if (used)
            return;
        if (GameManager.instance.time < cooldownTimer)
            return;
        cooldownTimer = GameManager.instance.time + cooldown + duration;
        TriggerCooldownDisplay();
        inventoryVar.StartCoroutine(UseWeapon(user, direction));
    }

    public virtual IEnumerator UseWeapon(GameObject user, Vector2 direction)
    {
        used = true;
        if(damageDelay > 0)
            yield return new WaitForSeconds(damageDelay);
        foreach (var target in GetTargets(user, direction))
        {
            if(target.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                healthSystem.health -= damage;
                if (singleHit) break;
            }
        }
        durability--;
        if (duration - damageDelay > 0)
            yield return new WaitForSeconds(duration - damageDelay);
        used = false;
    }

    public Collider2D[] GetTargets(GameObject user, Vector2 direction)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(user.transform.position, radius, targetLayer);
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float halfAngle = angle / 2f;

        List<Collider2D> validTargets = new List<Collider2D>();
        foreach (var collider in colliders)
        {
            Vector2 objectDirection = ((Vector2)collider.transform.position - (Vector2)user.transform.position).normalized;
            float objectAngle = Mathf.Atan2(objectDirection.y, objectDirection.x) * Mathf.Rad2Deg;
            if(collider.gameObject != user)
                if (objectAngle < lookAngle + halfAngle && objectAngle > lookAngle - halfAngle) validTargets.Add(collider);
        }
        
        return validTargets.ToArray();
    }
}
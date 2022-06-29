using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Weapon", menuName = "Items/Projectile Weapon")]
public class ProjectileWeapon : DurabilityItem
{
    [Header("Projectile Weapon")]
    public float angle;
    [Min(0)] public float fireDelay;
    [Min(0)] public float duration;
    [Min(0)] public float projectileInterval;
    [Min(1)] public int numberOfProjectiles;
    public Projectile projectilePrefab;
    public Item ammo;

    public ProjectileWeapon(SaveFile saveFile, ProjectileWeapon item) : base(saveFile, item)
    {
        this.angle = item.angle;
        this.duration = Mathf.Clamp(item.duration, item.projectileInterval * item.numberOfProjectiles + item.fireDelay, int.MaxValue);
        this.fireDelay = item.fireDelay;
        this.projectileInterval = item.projectileInterval;
        this.numberOfProjectiles = item.numberOfProjectiles;
        this.projectilePrefab = item.projectilePrefab;
        this.ammo = item.ammo;
    }

    public ProjectileWeapon(ProjectileWeapon item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
        this.angle = item.angle;
        this.duration = Mathf.Clamp(item.duration, item.projectileInterval * item.numberOfProjectiles + item.fireDelay, int.MaxValue);
        this.fireDelay = item.fireDelay;
        this.projectileInterval = item.projectileInterval;
        this.numberOfProjectiles = item.numberOfProjectiles;
        this.projectilePrefab = item.projectilePrefab;
        this.ammo = item.ammo;
    }

    public override Item Create(int amount, Inventory inventory)
    {
        ProjectileWeapon item = new ProjectileWeapon(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public override Item Create(SaveFile saveFile)
    {
        ProjectileWeapon item = new ProjectileWeapon(saveFile, this);
        item.name = this.name;
        return item;
    }

    public override string GetInformation()
    {
        return "<b>" + this.name + "</b>\n"
            + description + "\n\n"
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
        float startingAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - angle / 2f;
        used = true;
        if (fireDelay > 0)
            yield return new WaitForSeconds(fireDelay);
        WaitForSeconds wait = new WaitForSeconds(projectileInterval);
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            if (ammo)
            {
                Item item = inventoryVar.GetItem(ammo, inventoryVar);
                if (!item)
                {
                    yield return new WaitForSeconds(projectileInterval * (numberOfProjectiles - i + 1));
                    if (duration - projectileInterval * numberOfProjectiles > 0)
                        yield return new WaitForSeconds(duration - projectileInterval * numberOfProjectiles);
                    used = false;
                    yield break;
                }
                item.amount--;
            }

            Projectile projectile = Instantiate(projectilePrefab, user.transform.position, new Quaternion(0,0,0,0));
            if(numberOfProjectiles > 1)
            {
                float projectileAngle = startingAngle + angle / (numberOfProjectiles - 1) * i;
                float rad = projectileAngle * Mathf.Deg2Rad;
                projectile.Setup(new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)), user);
            }
            else
                projectile.Setup(direction, user);
            if(projectileInterval > 0)
                yield return wait;
        }
        durability--;
        if (duration - projectileInterval * numberOfProjectiles > 0)
            yield return new WaitForSeconds(duration - projectileInterval * numberOfProjectiles);
        used = false;
    }
}

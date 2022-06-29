using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class DropItem : MonoBehaviour
{
    [SerializeField] private float cooldown = 1;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Item item;
    private int amount;
    private bool fullItem;
    private float timer;

    public void Setup(Vector2 velocity, Item item, int amount = 0, bool fullItem = false)
    {
        this.item = item;
        this.amount = amount;
        this.fullItem = fullItem;
        timer = GameManager.instance.time + cooldown;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.dropSprite;
        rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(velocity, ForceMode2D.Impulse);
    }

    public void PickupItem(Inventory inventory)
    {
        if (GameManager.instance.time < timer) return;

        if (fullItem)
        {
            if (inventory.AddItem(item, inventory))
                Destroy(this.gameObject);
        }
        else
        {
            if (inventory.AddItem(item, amount, inventory) <= 0)
                Destroy(this.gameObject);
        }
    }
}

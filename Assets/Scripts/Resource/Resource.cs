using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField][Min(1)] private float maxHealth = 1;
    [SerializeField] private Sprite destroyedSprite;
    [SerializeField] private bool collisionWhenDestroyed;
    [SerializeField] private bool respawns;
    [SerializeField] private float respawnTimeMin;
    [SerializeField] private ItemAmount[] dropItems;
    [SerializeField] private DropItem dropItemPrefab;

    private Sprite fullSprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;
    private Animator animator;

    private float health;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        collider = this.GetComponent<Collider2D>();
        animator = this.GetComponent<Animator>();
        fullSprite = spriteRenderer.sprite;
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Hit(float damage, float chanceBonus = 0)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health <= 0) Destroy(chanceBonus);
    }

    public void Destroy(float chanceBonus)
    {
        animator.enabled = false;
        spriteRenderer.sprite = destroyedSprite;
        collider.enabled = collisionWhenDestroyed;
        DropItems(chanceBonus);
        if (respawns) StartCoroutine(Respawn());
    }

    public void DropItems(float chanceBonus)
    {
        foreach (var item in dropItems)
        {
            int amount = item.GetAmountChance(chanceBonus);
            if (amount > 0)
            {
                DropItem dropItem = Instantiate(dropItemPrefab, this.transform.position, new Quaternion(0,0,0,0));
                dropItem.Setup(new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f)).normalized * 5, item.item, amount);
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTimeMin * 60f);
        health = maxHealth;
        animator.enabled = true;
        spriteRenderer.sprite = fullSprite;
        collider.enabled = true;
    }
}
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

    private float respawnTimer;
    private bool respawning = false;
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
        if(animator) animator.enabled = false;
        spriteRenderer.sprite = destroyedSprite;
        collider.enabled = collisionWhenDestroyed;
        DropItems(chanceBonus);
        if (respawns)
        {
            respawning = true;
            respawnTimer = GameManager.instance.time + respawnTimeMin * 60f;
        }
    }

    public void DropItems(float chanceBonus)
    {
        foreach (var item in dropItems)
        {
            int amount = item.GetAmountChance(chanceBonus);
            for (int i = 0; i < amount; i++)
            {
                DropItem dropItem = Instantiate(dropItemPrefab, this.transform.position, new Quaternion(0,0,0,0));
                dropItem.Setup(new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f)).normalized * 5, item.item, 1);
            }
        }
    }

    private void Update()
    {
        if (!respawning) return;
        if (GameManager.instance.time > respawnTimer)
        {
            health = maxHealth;
            if (animator) animator.enabled = true;
            spriteRenderer.sprite = fullSprite;
            collider.enabled = true;
        }
    }
}
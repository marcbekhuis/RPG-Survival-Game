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

    public void Hit(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health <= 0) Destroy();
    }

    public void Destroy()
    {
        animator.enabled = false;
        spriteRenderer.sprite = destroyedSprite;
        collider.enabled = collisionWhenDestroyed;
        if (respawns) StartCoroutine(Respawn());
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
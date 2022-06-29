using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Character Movement")]
    [SerializeField] protected float walkSpeed = 3;
    [SerializeField] protected float runSpeed = 3;
    [SerializeField] protected float runStaminaCost = 1;
    [SerializeField] private StaminaSystem staminaSystem;

    protected Rigidbody2D rigidbody;
    public Vector2 lastDirection;

    private PathFinding.PathTile onTile;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        lastDirection = Vector2.right;
    }

    public void Move(Vector2 direction, bool run = false)
    {
        direction = direction.normalized;
        float speed = walkSpeed;
        if (run && HasStamina())
        {
            speed = runSpeed;
            if(direction != Vector2.zero)
                staminaSystem.stamina -= runStaminaCost * Time.deltaTime;
        }
        rigidbody.velocity = direction * speed;
        if (direction != Vector2.zero) lastDirection = direction;

        if (PathFinding.instance)
        {
            PathFinding.PathTile oldOnTile = onTile;
            onTile = PathFinding.instance.SetTileCharacter(this.transform.position, this);

            if (oldOnTile != null)
            {
                if (oldOnTile != onTile)
                    if (oldOnTile.character == this)
                        oldOnTile.character = null;
            }
        }
    }

    public bool HasStamina()
    {
        if (!staminaSystem) return true;
        return staminaSystem.stamina > 0;
    }
}

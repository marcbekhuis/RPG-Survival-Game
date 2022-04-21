using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Character Movement")]
    [SerializeField] protected float walkSpeed = 3;
    [SerializeField] protected float runSpeed = 3;

    protected Rigidbody2D rigidbody;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, bool run = false)
    {
        direction = direction.normalized;
        float speed = walkSpeed;
        if (run) speed = runSpeed;
        rigidbody.velocity = direction * speed;
    }
}

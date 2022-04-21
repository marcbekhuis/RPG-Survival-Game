using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public static PlayerMovement instance;

    public Vector2 lastDirection;

    private void Awake()
    {
        base.Awake();
        if (instance) Destroy(this);
        else instance = this;

        lastDirection = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        InputMove();
    }

    private void InputMove()
    {
        if (GameManager.instance.playerInputBlocked)
        {
            Move(Vector2.zero);
            return;
        }

        Vector2 direction = new Vector2(InputManager.instance.GetAxisRaw(AxisActions.Horizontal), InputManager.instance.GetAxisRaw(AxisActions.Vertical));
        if (direction != Vector2.zero) lastDirection = direction;
        Move(direction, InputManager.instance.GetKey(InputActions.Run));
    }
}
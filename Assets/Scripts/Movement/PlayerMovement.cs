using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
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
        Move(direction, InputManager.instance.GetKey(InputActions.Run));
    }
}
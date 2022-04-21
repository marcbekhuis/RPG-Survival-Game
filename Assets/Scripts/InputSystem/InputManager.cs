using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public InputSystem keyboardInput;
    public InputSystem ControllerInput;

    public InputSystem lastUsedInput;

    private new void Awake()
    {
        base.Awake();
        keyboardInput = new InputKeyboard();
        ControllerInput = new InputController();
    }

    public bool GetKey(InputActions action)
    {
        if (keyboardInput.GetKey(action))
        {
            lastUsedInput = keyboardInput;
            return true;
        }
        else if (ControllerInput.GetKey(action))
        {
            lastUsedInput = ControllerInput;
            return true;
        }
        return false;
    }

    public bool GetKeyDown(InputActions action)
    {
        if (keyboardInput.GetKeyDown(action))
        {
            lastUsedInput = keyboardInput;
            return true;
        }
        else if (ControllerInput.GetKeyDown(action))
        {
            lastUsedInput = ControllerInput;
            return true;
        }
        return false;
    }

    public bool GetKeyUp(InputActions action)
    {
        if (keyboardInput.GetKeyUp(action))
        {
            lastUsedInput = keyboardInput;
            return true;
        }
        else if (ControllerInput.GetKeyUp(action))
        {
            lastUsedInput = ControllerInput;
            return true;
        }
        return false;
    }

    public float GetAxis(AxisActions action)
    {
        float keyboardValue = keyboardInput.GetAxis(action);
        float controllerValue = ControllerInput.GetAxis(action);

        if (Mathf.Abs(keyboardValue) > Mathf.Abs(controllerValue))
        {
            lastUsedInput = keyboardInput;
            return keyboardValue;
        }
        else
        {
            lastUsedInput = ControllerInput;
            return controllerValue;
        }
    }

    public float GetAxisRaw(AxisActions action)
    {
        float keyboardValue = keyboardInput.GetAxisRaw(action);
        float controllerValue = ControllerInput.GetAxisRaw(action);

        if (Mathf.Abs(keyboardValue) > Mathf.Abs(controllerValue))
        {
            lastUsedInput = keyboardInput;
            return keyboardValue;
        }
        else
        {
            lastUsedInput = ControllerInput;
            return controllerValue;
        }
    }
}

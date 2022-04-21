using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : InputSystem
{
    public InputController()
    {
        inputName = "Controller";
        axises = new Dictionary<AxisActions, string>
    {
        {AxisActions.Horizontal, "Horizontal" },
        {AxisActions.Vertical, "Vertical" },
        {AxisActions.MouseScrollWheel, "Mouse ScrollWheel" },
    };
        keybinds = new Dictionary<InputActions, List<KeyCode>>
    {
        {InputActions.Run, new List<KeyCode> {KeyCode.LeftShift, KeyCode.RightShift }},
        {InputActions.MoveItem, new List<KeyCode> {KeyCode.Mouse0 }},
        {InputActions.ItemInteraction, new List<KeyCode> {KeyCode.Mouse1 }},
        {InputActions.ToggleInventory, new List<KeyCode> {KeyCode.Tab }},
        {InputActions.UseTool, new List<KeyCode> {KeyCode.Joystick1Button0 }},
        {InputActions.TogglePause, new List<KeyCode> {KeyCode.Escape }},
    };
        Load();
    }
}

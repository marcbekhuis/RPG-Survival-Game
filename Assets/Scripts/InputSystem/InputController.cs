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
        {InputActions.Run, new List<KeyCode> {KeyCode.JoystickButton8}},
        {InputActions.MoveItem, new List<KeyCode> {KeyCode.JoystickButton0 }},
        {InputActions.ItemInteraction, new List<KeyCode> {KeyCode.JoystickButton1 }},
        {InputActions.ToggleInventory, new List<KeyCode> {KeyCode.JoystickButton2 }},
        {InputActions.UseTool, new List<KeyCode> {KeyCode.JoystickButton4 }},
        {InputActions.TogglePause, new List<KeyCode> {KeyCode.JoystickButton7 }},
        {InputActions.DropItem, new List<KeyCode> {KeyCode.JoystickButton3 }},
        {InputActions.ToggleCraftingMenu, new List<KeyCode> {KeyCode.JoystickButton6 }},
    };
        Load();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyboard : InputSystem
{
    public override float GetAxis(AxisActions action)
    {
        switch (action)
        {
            case AxisActions.Horizontal:
                return System.Convert.ToInt32(base.GetKey(InputActions.Right)) - System.Convert.ToInt32(base.GetKey(InputActions.Left));
            case AxisActions.Vertical:
                return System.Convert.ToInt32(base.GetKey(InputActions.Forwards)) - System.Convert.ToInt32(base.GetKey(InputActions.Backwards));
            default:
                return base.GetAxis(action);
        }
    }

    public override float GetAxisRaw(AxisActions action)
    {
        switch (action)
        {
            case AxisActions.Horizontal:
                return System.Convert.ToInt32(base.GetKey(InputActions.Right)) - System.Convert.ToInt32(base.GetKey(InputActions.Left));
            case AxisActions.Vertical:
                return System.Convert.ToInt32(base.GetKey(InputActions.Forwards)) - System.Convert.ToInt32(base.GetKey(InputActions.Backwards));
            default:
                return base.GetAxis(action);
        }
    }

    public InputKeyboard()
    {
        inputName = "Keyboard";
        keybinds = new Dictionary<InputActions, List<KeyCode>>
    {
        {InputActions.Forwards, new List<KeyCode> {KeyCode.W } },
        {InputActions.Backwards, new List<KeyCode> {KeyCode.S }},
        {InputActions.Left, new List<KeyCode> {KeyCode.A }},
        {InputActions.Right, new List<KeyCode> {KeyCode.D }},
        {InputActions.Run, new List<KeyCode> {KeyCode.LeftShift, KeyCode.RightShift }},
        {InputActions.MoveItem, new List<KeyCode> {KeyCode.Mouse0 }},
        {InputActions.ItemInteraction, new List<KeyCode> {KeyCode.Mouse1 }},
        {InputActions.ToggleInventory, new List<KeyCode> {KeyCode.Tab }},
        {InputActions.SelectItem1, new List<KeyCode> {KeyCode.Alpha1 }},
        {InputActions.SelectItem2, new List<KeyCode> {KeyCode.Alpha2 }},
        {InputActions.SelectItem3, new List<KeyCode> {KeyCode.Alpha3 }},
        {InputActions.SelectItem4, new List<KeyCode> {KeyCode.Alpha4 }},
        {InputActions.SelectItem5, new List<KeyCode> {KeyCode.Alpha5 }},
        {InputActions.SelectItem6, new List<KeyCode> {KeyCode.Alpha6 }},
        {InputActions.SelectItem7, new List<KeyCode> {KeyCode.Alpha7 }},
        {InputActions.SelectItem8, new List<KeyCode> {KeyCode.Alpha8 }},
        {InputActions.SelectItem9, new List<KeyCode> {KeyCode.Alpha9 }},
        {InputActions.SelectItem0, new List<KeyCode> {KeyCode.Alpha0 }},
        {InputActions.UseTool, new List<KeyCode> {KeyCode.Mouse0 }},
        {InputActions.TogglePause, new List<KeyCode> {KeyCode.Escape }},
        {InputActions.DropItem, new List<KeyCode> {KeyCode.Q }},
        {InputActions.ToggleCraftingMenu, new List<KeyCode> {KeyCode.C }},
    };
        axises = new Dictionary<AxisActions, string>
    {
        {AxisActions.Horizontal, "Horizontal" },
        {AxisActions.Vertical, "Vertical" },
        {AxisActions.MouseScrollWheel, "Mouse ScrollWheel" },
    };
        Load();
    }
}

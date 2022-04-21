using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem
{
    protected string inputName;

    public Dictionary<InputActions, List<KeyCode>> keybinds;

    public Dictionary<AxisActions, string> axises;

    public InputSystem()
    {
        inputName = "Default";
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
    };
        axises = new Dictionary<AxisActions, string>
    {
        {AxisActions.Horizontal, "Horizontal" },
        {AxisActions.Vertical, "Vertical" },
        {AxisActions.MouseScrollWheel, "Mouse ScrollWheel" },
    };
        Load();
    }

    public virtual bool GetKey(InputActions action)
    {
        if (!keybinds.ContainsKey(action)) return false;

        foreach (var keyCode in keybinds[action])
        {
            if (Input.GetKey(keyCode)) return true;
        }
        return false;
    }

    public virtual bool GetKeyDown(InputActions action)
    {
        if (!keybinds.ContainsKey(action)) return false;

        foreach (var keyCode in keybinds[action])
        {
            if (Input.GetKeyDown(keyCode)) return true;
        }
        return false;
    }

    public virtual bool GetKeyUp(InputActions action)
    {
        if (!keybinds.ContainsKey(action)) return false;

        foreach (var keyCode in keybinds[action])
        {
            if (Input.GetKeyUp(keyCode)) return true;
        }
        return false;
    }

    public virtual float GetAxis(AxisActions action)
    {
        if (!axises.ContainsKey(action)) return 0;

        return Input.GetAxis(axises[action]);
    }

    public virtual float GetAxisRaw(AxisActions action)
    {
        if (!axises.ContainsKey(action)) return 0;

        return Input.GetAxisRaw(axises[action]);
    }

    public void Save()
    {
        JsonSavingLoading.SaveData<SaveFile>(new SaveFile(keybinds), "Settings/Keybinds", inputName);
    }

    public void Load()
    {
        SaveFile inputSave = JsonSavingLoading.LoadData<SaveFile>("Settings/Keybinds", inputName);
        if (inputSave == null) return;

        keybinds.Clear();
        for (int i = 0; i < inputSave.actions.Length; i++)
        {
            List<KeyCode> keyCodes = new List<KeyCode>();
            foreach (var keycode in inputSave.keyCodes[i].Split(','))
            {
                keyCodes.Add(System.Enum.Parse<KeyCode>(keycode));
            }
            keybinds.Add(System.Enum.Parse<InputActions>(inputSave.actions[i]), keyCodes);
        }
    }

    public class SaveFile
    {
        public SaveFile(Dictionary<InputActions, List<KeyCode>> keybinds)
        {
            List<string> actionsList = new List<string>();
            List<string> keyCodesList = new List<string>();

            foreach (var keybind in keybinds)
            {
                actionsList.Add(keybind.Key.ToString());
                string keycode = "";

                foreach (var item in keybind.Value)
                {
                    keycode += item + ",";
                }

                keycode = keycode.Remove(keycode.Length - 1, 1);

                keyCodesList.Add(keycode);
            }

            actions = actionsList.ToArray();
            keyCodes = keyCodesList.ToArray();
        }

        public string[] actions;
        public string[] keyCodes;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputSettings : MonoBehaviour
{
    [SerializeField] private GameObject keybindPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Type type;
    
    protected InputSystem inputSystem;

    private Dictionary<InputActions, List<KeyCode>> settingKeybinds = new Dictionary<InputActions, List<KeyCode>>();
    private bool keycodeCheck = false;
    private InputActions changingAction;
    private int index = 0;
    private Button button;
    private Text keycodeText;

    private void Start()
    {
        switch (type)
        {
            case Type.Keyboard:
                inputSystem = InputManager.instance.keyboardInput;
                break;
            case Type.Controller:
                inputSystem = InputManager.instance.ControllerInput;
                break;
            default:
                inputSystem = InputManager.instance.keyboardInput;
                break;
        }
        CopyDictionary();

        foreach (var keybind in settingKeybinds)
        {
            GameObject keybindGameObject = Instantiate(keybindPrefab, parent);
            keybindGameObject.GetComponentInChildren<Text>().text = keybind.Key.ToString();
            Button button = keybindGameObject.GetComponentInChildren<Button>();
            Text keycodeText = button.GetComponentInChildren<Text>();
            keycodeText.text = keybind.Value[0].ToString();
            button.onClick.AddListener(() => ChangeKeybind(keycodeText, button, keybind.Key, 0));
        }
    }

    private void Update()
    {
        if (keycodeCheck)
        {
            foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keycode))
                {
                    settingKeybinds[changingAction][index] = keycode;
                    button.interactable = true;
                    keycodeText.text = keycode.ToString();
                    keycodeCheck = false;
                    break;
                }
            }
        }
    }

    private void ChangeKeybind(Text keycodeText, Button button, InputActions action, int index)
    {
        this.keycodeText = keycodeText;
        this.button = button;
        this.button.interactable = false;
        changingAction = action;
        this.index = index;
        keycodeCheck = true;
    }

    public void Cancel()
    {
        CopyDictionary();

        for (int i = 0; i < settingKeybinds.Count; i++)
        {
            GameObject keybindGameObject = parent.GetChild(i).gameObject;
            keybindGameObject.GetComponentInChildren<Text>().text = settingKeybinds.Keys.ElementAt(i).ToString();
            Button button = keybindGameObject.GetComponentInChildren<Button>();
            Text keycodeText = button.GetComponentInChildren<Text>();
            keycodeText.text = settingKeybinds.Values.ElementAt(i)[0].ToString();
        }
    }

    public void CopyDictionary()
    {
        settingKeybinds.Clear();
        foreach (var keybind in inputSystem.keybinds)
        {
            settingKeybinds.Add(keybind.Key, new List<KeyCode>(keybind.Value));
        }
    }

    public void Save()
    {
        inputSystem.keybinds.Clear();
        foreach (var keybind in settingKeybinds)
        {
            inputSystem.keybinds.Add(keybind.Key, new List<KeyCode>(keybind.Value));
        }
        inputSystem.Save();
    }

    public enum Type
    {
        Keyboard,
        Controller
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private Button disabledButton;
    [SerializeField] private Button[] activeButtons;

    private void OnEnable()
    {
        disabledButton.interactable = false;
        foreach (var button in activeButtons)
        {
            button.interactable = true;
        }
    }
}

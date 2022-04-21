using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : Singleton<MenuHandler>
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameUI;

    // Update is called once per frame
    private void Update()
    {
        Escape();
        if (InputManager.instance.GetKeyDown(InputActions.ToggleInventory)) ToggleInventory(!inventory.activeSelf);
    }

    public void Escape()
    {
        if (!InputManager.instance.GetKeyDown(InputActions.TogglePause)) return;

        if (inventory.activeSelf)
        {
            ToggleInventory(false);
            return;
        }
        TogglePauseMenu(!pauseMenu.activeSelf);
    }

    public void ToggleInventory(bool active)
    {
        GameManager.instance.menuOpen = active;
        inventory.SetActive(active);
    }

    public void TogglePauseMenu(bool active)
    {
        GameManager.instance.paused = active;
        pauseMenu.SetActive(active);
    }

    public void ToggleGameUI(bool active)
    {
        gameUI.SetActive(active);
    }
}

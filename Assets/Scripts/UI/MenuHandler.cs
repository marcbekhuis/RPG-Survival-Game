using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : Singleton<MenuHandler>
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject craftingMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private GameObject gameUI;

    public delegate void InventoryToggle(bool active);
    public event InventoryToggle onInventoryToggle = delegate { };

    // Update is called once per frame
    private void Update()
    {
        Escape();
        if (InputManager.instance.GetKeyDown(InputActions.ToggleInventory)) ToggleInventory(!inventory.activeSelf);
        if (InputManager.instance.GetKeyDown(InputActions.ToggleCraftingMenu)) ToggleCraftingMenu(!craftingMenu.activeSelf);
    }

    public void Escape()
    {
        if (!InputManager.instance.GetKeyDown(InputActions.TogglePause)) return;

        if (inventory.activeSelf)
        {
            ToggleInventory(false);
            return;
        }
        if (craftingMenu.activeSelf)
        {
            ToggleCraftingMenu(false);
            return;
        }
        TogglePauseMenu(!pauseMenu.activeSelf);
    }

    public void ToggleInventory(bool active)
    {
        if (GameManager.instance.paused)
            return;

        GameManager.instance.menuOpen = active;
        onInventoryToggle(active);
        inventory.SetActive(active);
        menuButtons.SetActive(active);
        craftingMenu.SetActive(false);
    }

    public void ToggleCraftingMenu(bool active)
    {
        if (GameManager.instance.paused)
            return;

        GameManager.instance.menuOpen = active;
        onInventoryToggle(false);
        craftingMenu.SetActive(active);
        menuButtons.SetActive(active);
        inventory.SetActive(false);
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

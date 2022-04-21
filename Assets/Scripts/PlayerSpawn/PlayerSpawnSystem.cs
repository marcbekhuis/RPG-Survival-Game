using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnSystem : Singleton<PlayerSpawnSystem>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string sceneName;
    [SerializeField] private Vector2 spawnpoint;
    [Space]
    [SerializeField] private InventoryDisplay inventoryDisplay;
    [SerializeField] private InventoryDisplay hotbarInventoryDisplay;
    [SerializeField] private HotbarDisplay hotbarDisplay;
    [SerializeField] private HealthDisplay healthDisplay;

    public void Spawn()
    {
        GameObject existingPlayer = GameObject.Find(playerPrefab.name);
        if (existingPlayer)
        {
            SetComponents(existingPlayer);
            return;
        }

        if(!SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        GameObject player = Instantiate(playerPrefab);
        SetComponents(player);
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(sceneName));
        player.transform.position = spawnpoint;
    }

    public void SetComponents(GameObject player)
    {
        player.GetComponent<PlayerSkin>().SetSkin();
        Inventory inventory = player.GetComponent<Inventory>();
        inventoryDisplay.inventory = inventory;
        HotbarInventory hotbarInventory = player.GetComponent<HotbarInventory>();
        hotbarInventoryDisplay.inventory = hotbarInventory;
        hotbarDisplay.inventory = hotbarInventory;
        hotbarInventory.FullSetup();
        inventory.FullSetup();
        player.GetComponent<HealthSystem>().onHealthChange += healthDisplay.UpdateDisplay;
        ItemInteractionMenuManager.instance.user = player;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class ScenePortal : MonoBehaviour
{
    [SerializeField] private string activateScene;
    [SerializeField] private string disableScene;
    [SerializeField] private Vector2 teleportPlayer;
    [SerializeField] private float cooldown = 1;

    private Collider2D collider;
    private bool teleported = false;
    private float timer;

    private void Start()
    {
        collider = this.GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        if (teleported)
        {
            timer = GameManager.instance.time + cooldown;
            teleported = false;
        }
    }

    private void Update()
    {
        if(GameManager.instance.time > timer && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        teleported = true;
        collider.isTrigger = false;
        foreach (var item in SceneManager.GetSceneByName(disableScene).GetRootGameObjects())
        {
            item.SetActive(false);
        }

        Scene scene = SceneManager.GetSceneByName(activateScene);

        if (scene.isLoaded)
        {
            foreach (var item in scene.GetRootGameObjects())
            {
                item.SetActive(true);
            }
        }
        else
            SceneManager.LoadSceneAsync(activateScene, LoadSceneMode.Additive);

        SceneManager.MoveGameObjectToScene(collision.gameObject, SceneManager.GetSceneByName(activateScene));
        collision.gameObject.SetActive(true);
        collision.transform.position = teleportPlayer;
    }
}

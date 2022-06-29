using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreCheck : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetSceneByName("Core").isLoaded) return;
        SceneManager.LoadSceneAsync("Core", LoadSceneMode.Additive);
    }
}
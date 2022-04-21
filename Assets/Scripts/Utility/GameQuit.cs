using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

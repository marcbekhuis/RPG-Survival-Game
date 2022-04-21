using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private float timeVar = 0;

    public bool pausedVar = false;
    public bool menuOpen = false;

    public float time { get => timeVar; }
    public bool playerInputBlocked { get => paused || menuOpen; }
    public bool paused { get => pausedVar; 
        set
        {
            pausedVar = value;
            onTogglePause(pausedVar);
        }
    }

    public delegate void StateChanged(bool value);
    public event StateChanged onTogglePause = delegate { };

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        if (paused) return;
        timeVar += Time.deltaTime;
    }
}

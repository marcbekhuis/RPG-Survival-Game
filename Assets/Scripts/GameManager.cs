using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private float timeVar = 0;

    private bool pausedVar = false;
    public bool menuOpen = false;
    public bool respawning = false;

    public float time { get => timeVar; }
    public bool playerInputBlocked { get => pausedVar || menuOpen || respawning; }
    public bool gameActive { get => !respawning; }
    public bool paused { get => pausedVar || respawning; 
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

    private void Start()
    {
        respawning = true;
    }

    private void UpdateTime()
    {
        if (paused) return;
        timeVar += Time.deltaTime;
    }
}

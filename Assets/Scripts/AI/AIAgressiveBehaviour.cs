using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgressiveBehaviour : AIWandering
{
    private void Start()
    {
        SetupWandering();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused)
            return;

        Wandering();
    }
}

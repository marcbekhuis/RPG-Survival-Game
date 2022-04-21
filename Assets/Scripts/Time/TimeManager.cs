using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [SerializeField] private float cycleDuration = 10;

    [SerializeField] private float timeVar = 0;

    public delegate void TimeChange(float time);
    public event TimeChange onTimeChange = delegate { };

    public float dayTime
    {
        get => timeVar;
        set
        {
            timeVar = value;
            if (timeVar >= 24) timeVar -= 24;
            else if (timeVar < 0) timeVar += 24;
            onTimeChange(timeVar);
        }
    }

    private void Update()
    {
        if (GameManager.instance.paused) return;
        dayTime += 24 * Time.deltaTime / (cycleDuration * 60f);
    }
}

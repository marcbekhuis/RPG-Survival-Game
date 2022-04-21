using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private AnimationCurve brightness;
    [SerializeField] private Gradient color;
    [SerializeField] private Light2D light;

    private void Start()
    {
        TimeManager.instance.onTimeChange += UpdateTime;
    }

    public void UpdateTime(float time)
    {
        light.intensity = brightness.Evaluate(time);
        light.color = color.Evaluate(1f / 24f);
    }
}

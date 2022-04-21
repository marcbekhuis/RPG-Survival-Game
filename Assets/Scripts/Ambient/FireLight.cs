using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireLight : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private AnimationCurve brightness;
    [SerializeField] private AnimationCurve radius;

    private float brightnessTime = 0;
    private float radiusTime = 0;
    private float brightnessDuration;
    private float radiusDuration;
    [SerializeField] private Light2D light;

    private void Start()
    {
        brightnessDuration = brightness[brightness.length - 1].time;
        radiusDuration = radius[radius.length - 1].time;
        brightnessTime = Random.Range(0, brightnessDuration);
        radiusTime = Random.Range(0, radiusDuration);
        ChangeLight();
    }

    // Update is called once per frame
    void Update()
    {
        brightnessTime += Time.deltaTime;
        radiusTime += Time.deltaTime;
        if (brightnessTime > brightnessDuration) brightnessTime -= brightnessDuration;
        if (radiusTime > radiusDuration) radiusTime -= radiusTime;
        ChangeLight();
    }

    private void ChangeLight()
    {
        light.intensity = brightness.Evaluate(brightnessTime);
        light.pointLightOuterRadius = radius.Evaluate(radiusTime);
        light.color = gradient.Evaluate(brightnessTime / ((brightnessDuration + radiusDuration) / 2f));
    }
}

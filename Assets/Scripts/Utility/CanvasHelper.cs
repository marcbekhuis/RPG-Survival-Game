using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasHelper : Singleton<CanvasHelper>
{
    private CanvasScaler canvasScaler;

    private new void Awake()
    {
        base.Awake();
        canvasScaler = this.GetComponent<CanvasScaler>();
    }

    public float CanvasScale()
    {
        return canvasScaler.scaleFactor;
    }
}

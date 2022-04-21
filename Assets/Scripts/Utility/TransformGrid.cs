using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformGrid
{
    static float gridStep = 0.03125f;

    public static Vector3 WorldPositionToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridStep) * gridStep;
        float y = Mathf.Round(position.y / gridStep) * gridStep;
        return new Vector3(x,y,0);
    }

    public static Vector3 WorldPositionToGridXOffset(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridStep) * gridStep + gridStep / 2f;
        float y = Mathf.Round(position.y / gridStep) * gridStep;
        return new Vector3(x, y, 0);
    }

    public static Vector3 WorldPositionToGridYOffset(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridStep) * gridStep;
        float y = Mathf.Round(position.y / gridStep) * gridStep + gridStep / 2f;
        return new Vector3(x, y, 0);
    }

    public static Vector3 WorldPositionToGridOffset(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridStep) * gridStep + gridStep / 2f;
        float y = Mathf.Round(position.y / gridStep) * gridStep + gridStep / 2f;
        return new Vector3(x, y, 0);
    }
}

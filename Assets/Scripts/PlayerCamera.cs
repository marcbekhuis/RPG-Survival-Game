using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static Camera camera;

    private void Awake()
    {
        camera = this.GetComponent<Camera>();
    }
}

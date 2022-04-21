using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float speed = 10;

    private void Start()
    {
        this.transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused) return;
        this.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}

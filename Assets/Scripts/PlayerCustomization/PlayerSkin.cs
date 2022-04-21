using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;

    public void SetSkin()
    {
        body.color = PlayerCustomization.instance.bodyColor;
    }
}

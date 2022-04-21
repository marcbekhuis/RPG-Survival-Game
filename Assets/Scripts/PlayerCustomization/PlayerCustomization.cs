using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomization : Singleton<PlayerCustomization>
{
    public Color bodyColor;

    [SerializeField] private Image bodyPreview;

    // Start is called before the first frame update
    void Start()
    {
        if (bodyPreview) bodyPreview.color = bodyColor;
    }

    public void SetBodyColor(string hex)
    {
        if(ColorUtility.TryParseHtmlString("#"+hex, out bodyColor))
        {
            if (bodyPreview) bodyPreview.color = bodyColor;
        }
    }
}

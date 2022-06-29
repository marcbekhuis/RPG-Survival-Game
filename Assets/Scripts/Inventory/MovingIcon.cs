using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingIcon : Singleton<MovingIcon>
{
    [SerializeField] private Image movingIcon;

    // Start is called before the first frame update
    void Start()
    {
        movingIcon.gameObject.SetActive(false);
    }

    public void ToggleIcon(bool active, Sprite icon)
    {
        movingIcon.gameObject.SetActive(active);
        movingIcon.sprite = icon;
    }

    public void MoveIcon()
    {
        movingIcon.transform.position = Input.mousePosition;
    }
}

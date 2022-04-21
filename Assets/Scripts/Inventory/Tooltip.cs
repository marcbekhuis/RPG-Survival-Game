using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text informationText;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        ChangeAnchor();
        this.transform.position = Input.mousePosition;
    }

    public void ChangeAnchor()
    {
        if(Input.mousePosition.x + rectTransform.rect.width * CanvasHelper.instance.CanvasScale() >= Screen.width)
            rectTransform.pivot = new Vector2(1, rectTransform.pivot.y);
        else
            rectTransform.pivot = new Vector2(0, rectTransform.pivot.y);

        if (Input.mousePosition.y - rectTransform.rect.height * CanvasHelper.instance.CanvasScale() <= 0)
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 0);
        else
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1);
    }
}

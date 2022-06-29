using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionMenu : MonoBehaviour
{
    public Button splitButton;
    public Button useButton;
    public Button dropButton;

    private bool justOpened = false;
    private RectTransform rectTransform;

    private void Start()
    {
        justOpened = true;
        this.gameObject.SetActive(false);
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
        }
        if (Input.anyKeyDown)
            if (!IsOverPanel(Input.mousePosition))
                ItemInteractionMenuManager.instance.Deactivate();
    }

    public bool IsOverPanel(Vector2 position)
    {
        Vector2 size = rectTransform.rect.size * CanvasHelper.instance.CanvasScale();
        float highPosition = rectTransform.position.y;
        float lowPosition = rectTransform.position.y - size.y;
        float leftPosition = rectTransform.position.x;
        float rightPosition = rectTransform.position.x + size.x;
        if (position.x > leftPosition && position.x < rightPosition)
        {
            if (position.y > lowPosition && position.y < highPosition)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeAnchor()
    {
        if (Input.mousePosition.x + rectTransform.rect.width * CanvasHelper.instance.CanvasScale() >= Screen.width)
            rectTransform.pivot = new Vector2(1, rectTransform.pivot.y);
        else
            rectTransform.pivot = new Vector2(0, rectTransform.pivot.y);

        if (Input.mousePosition.y - rectTransform.rect.height * CanvasHelper.instance.CanvasScale() <= 0)
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 0);
        else
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1);
    }
}

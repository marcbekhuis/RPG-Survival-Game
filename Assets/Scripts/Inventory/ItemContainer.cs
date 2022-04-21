using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private ItemDisplay itemDisplayPrefab;
    [SerializeField] private Image backgroundImage;

    private Item itemVar;
    private ItemDisplay itemDisplay;
    private RectTransform rectTransform;

    public Item item
    {
        get => itemVar;
        set
        {
            itemVar = value;
            if (itemVar)
            {
                if (itemDisplay)
                {
                    itemDisplay.item = itemVar;
                    itemDisplay.gameObject.SetActive(true);
                }
                else
                {
                    itemDisplay = Instantiate(itemDisplayPrefab, this.transform);
                    itemDisplay.item = itemVar;
                }
            }
            else
            {
                if (itemDisplay) itemDisplay.gameObject.SetActive(false);
            }
        }
    }

    public bool IsOver(Vector2 position)
    {
        Vector2 halfSize = rectTransform.rect.size * 0.5f * CanvasHelper.instance.CanvasScale();
        float highPosition = rectTransform.position.y + halfSize.y;
        float lowPosition = rectTransform.position.y - halfSize.y;
        float leftPosition = rectTransform.position.x - halfSize.x;
        float rightPosition = rectTransform.position.x + halfSize.x;
        if (position.x > leftPosition && position.x < rightPosition)
        {
            if (position.y > lowPosition && position.y < highPosition)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeBackgroundImage(Sprite background)
    {
        backgroundImage.sprite = background;
    }

    public void ChangeBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }

    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }
}

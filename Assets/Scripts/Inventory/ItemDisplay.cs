using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text amountText;
    [SerializeField] private RectTransform durabilityBar;
    [SerializeField] private RectTransform durabilityFill;
    private RectTransform rectTransform;

    private Item itemVar;

    public Item item
    {
        get => itemVar;
        set
        {
            if (itemVar)
            {
                itemVar.onAmountChange -= UpdateAmount;
                if (item.TryGetType<DurabilityItem>(out DurabilityItem durabilityItem)) durabilityItem.onDurabilityChanged -= UpdateDurabilityBar;
            }

            itemVar = value;
            iconImage.sprite = itemVar.icon;
            amountText.gameObject.SetActive(itemVar.stackSize > 1);
            amountText.text = itemVar.amount.ToString();

            if (itemVar)
            {
                itemVar.onAmountChange += UpdateAmount;
                if (item.TryGetType<DurabilityItem>(out DurabilityItem durabilityItem))
                {
                    durabilityBar.gameObject.SetActive(true);
                    durabilityItem.onDurabilityChanged += UpdateDurabilityBar;
                    UpdateDurabilityBar(durabilityItem.durability, durabilityItem.maxDurability);
                }
                else durabilityBar.gameObject.SetActive(false);
                if(this.gameObject.activeSelf)
                    if (!TooltipManager.instance.HasItemdisplay(this)) 
                        TooltipManager.instance.AddItemdisplay(this);
            }
            else TooltipManager.instance.RemoveItemdisplay(this);
        }
    }

    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (itemVar)
        {
            if (!TooltipManager.instance.HasItemdisplay(this))
                TooltipManager.instance.AddItemdisplay(this);
        }
    }

    private void OnDisable()
    {
        TooltipManager.instance.RemoveItemdisplay(this);
    }

    private void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    private void UpdateDurabilityBar(float durability, float maxDurability)
    {
        durabilityFill.sizeDelta = new Vector2(durabilityBar.rect.width * (durability / maxDurability), durabilityBar.rect.height);
    }

    public bool IsOver(Vector2 position)
    {
        if (!rectTransform) return false;

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
}

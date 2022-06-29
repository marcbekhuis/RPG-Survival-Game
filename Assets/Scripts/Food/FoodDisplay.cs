using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDisplay : MonoBehaviour
{
    [SerializeField] private float hideFoodCooldown;
    [SerializeField] private Image foodFill;
    [SerializeField] private UIAnimations animations;

    private float hideTimer;
    private bool Hiden;

    public void UpdateDisplay(float food, float maxFood)
    {
        foodFill.fillAmount = food / maxFood;
        animations.ResetElement();
        hideTimer = GameManager.instance.time + hideFoodCooldown;
        Hiden = false;
    }

    private void Update()
    {
        if (Hiden) return;
        if (GameManager.instance.time > hideTimer)
        {
            Hiden = true;
            animations.TriggerMoveToOffset();
        }
    }
}

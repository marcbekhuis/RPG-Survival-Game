using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplay : MonoBehaviour
{
    [SerializeField] private float hideStaminaCooldown;
    [SerializeField] private Image staminaFill;
    [SerializeField] private UIAnimations animations;

    private float hideTimer;
    private bool Hiden;

    public void UpdateDisplay(float stamina, float maxStamina)
    {
        staminaFill.fillAmount = stamina / maxStamina;
        animations.ResetElement();
        hideTimer = GameManager.instance.time + hideStaminaCooldown;
        Hiden = false;
    }

    private void Update()
    {
        if (Hiden) return;
        if (GameManager.instance.time > hideTimer)
        {
            Hiden = true;
            animations.TriggerBlendToHidden();
        }
    }
}

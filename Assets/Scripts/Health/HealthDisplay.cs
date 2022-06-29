using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private float hideHealthCooldown;
    [SerializeField] private Image healthFill;
    [SerializeField] private UIAnimations animations;

    private float hideTimer;
    private bool Hiden;

    public void UpdateDisplay(float health, float maxHealth)
    {
        healthFill.fillAmount = health / maxHealth;
        animations.ResetElement();
        hideTimer = GameManager.instance.time + hideHealthCooldown;
        Hiden = false;
    }

    private void Update()
    {
        if (Hiden) return;
        if(GameManager.instance.time > hideTimer)
        {
            Hiden = true;
            animations.TriggerMoveToOffset();
        }
    }
}

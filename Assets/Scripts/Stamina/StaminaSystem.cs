using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [SerializeField] [Min(0)] private float maxStamina;
    [SerializeField] private float staminaRegenCooldown = 1;
    [SerializeField] private float staminaRegenPerSec = 1;
    [SerializeField] private float foodConsumePerSec = 1;
    [SerializeField] private FoodSystem foodSystem;

    private float staminaVar;
    private float regenTimer;

    public delegate void StaminaChange(float stamina, float maxStamina);
    public event StaminaChange onStaminaChange = delegate { };

    public float stamina
    {
        get => staminaVar;
        set
        {
            float oldStamina = staminaVar;
            if(value < staminaVar) 
                regenTimer = GameManager.instance.time + staminaRegenCooldown;

            staminaVar = Mathf.Clamp(value, 0, maxStamina);
            if(oldStamina != staminaVar)
                onStaminaChange(staminaVar, maxStamina);
            if(stamina > oldStamina)
                foodSystem.food -= foodConsumePerSec * Time.deltaTime;
        }
    }

    public void SetStaminaToMax()
    {
        stamina = maxStamina;
        regenTimer = GameManager.instance.time;
    }

    private void Update()
    {
        if (GameManager.instance.paused) return;

        if (GameManager.instance.time > regenTimer)
        {
            if(foodSystem.food > 0)
                stamina += staminaRegenPerSec * Time.deltaTime;
        }
    }
}

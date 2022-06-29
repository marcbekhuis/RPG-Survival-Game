using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSystem : MonoBehaviour
{
    [SerializeField] [Min(0)] private float maxFood;
    [SerializeField] private float hungerDamagePerSec = 2;
    [SerializeField] private HealthSystem healthSystem;

    private float foodVar;

    public delegate void FoodChange(float food, float maxFood);
    public event FoodChange onFoodChange = delegate { };

    public float food
    {
        get => foodVar;
        set
        {
            float oldFood = foodVar;

            foodVar = Mathf.Clamp(value, 0, maxFood);
            if (oldFood != foodVar)
                onFoodChange(foodVar, maxFood);
        }
    }

    public void SetFoodToMax()
    {
        food = maxFood;
    }

    private void Update()
    {
        if (GameManager.instance.paused) return;
        if (food <= 0) healthSystem.health -= hungerDamagePerSec * Time.deltaTime;
    }
}

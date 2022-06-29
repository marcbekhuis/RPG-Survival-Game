using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField][Min(0)] private float maxHealth;

    private float healthVar;

    public delegate void HealthChange(float health, float maxHealth);
    public event HealthChange onHealthChange = delegate { };

    public float health
    { 
        get => healthVar;
        set
        {
            float oldHealth = healthVar;
            healthVar = Mathf.Clamp(value, 0, maxHealth);
            if(healthVar != oldHealth)
                onHealthChange(healthVar, maxHealth);
        }
    }

    public void SetHealthToMax()
    {
        health = maxHealth;
    }
}

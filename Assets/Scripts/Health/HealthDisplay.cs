using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    public void UpdateDisplay(float health, float maxHealth)
    {
        healthFill.fillAmount = health / maxHealth;
    }
}

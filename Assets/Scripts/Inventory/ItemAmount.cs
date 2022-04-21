using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemAmount
{
    public Item item;
    [SerializeField] private float chance = 0;
    [SerializeField] private int minAmount = 0;
    [SerializeField] private int maxAmount = 1;

    public int GetAmount()
    {
        return Random.Range(minAmount, maxAmount);
    }

    public int GetAmountChance(float bonusChance = 0)
    {
        if (Random.Range(0, 100) + bonusChance < chance)
            return Random.Range(minAmount, maxAmount);
        else
            return 0;
    }
}

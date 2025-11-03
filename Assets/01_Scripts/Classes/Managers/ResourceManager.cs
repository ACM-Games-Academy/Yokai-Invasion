using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Action UpdateWood;
    public Action UpdateFood;
    public Action UpdateGold;
    
    private int food = 0;
    private int wood = 0;
    private int gold = 0;

    public int CurrentFood() => food;
    public int CurrentWood() => wood;
    public int CurrentGold() => gold;

    public void IncreaseFood(int amount)
    {
        food += amount;
        UpdateFood?.Invoke();
    }
    public void IncreaseWood(int amount)
    {
        wood += amount;
        UpdateWood?.Invoke();
    }
    public void IncreaseGold(int amount)
    {
        gold += amount;
        UpdateGold?.Invoke();
    }

    public void DecreaseFood(int amount)
    {
        food -= amount;
        UpdateFood?.Invoke();
    }

    public void DecreaseWood(int amount)
    {
        wood -= amount;
        UpdateWood?.Invoke();
    }

    public void DecreaseGold(int amount)
    {
        gold -= amount;
        UpdateGold?.Invoke();
    }
}

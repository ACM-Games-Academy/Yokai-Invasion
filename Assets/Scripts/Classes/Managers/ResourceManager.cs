using NUnit.Framework;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int food = 0;
    private int wood = 0;
    private int gold = 0;

    public int CurrentFood() => food;
    public int CurrentWood() => wood;
    public int CurrentGold() => gold;

    public void IncreaseFood(int amount) => food += amount;
    public void IncreaseWood(int amount) => wood += amount;
    public void IncreaseGold(int amount) => gold += amount;

    public void DecreaseFood(int amount)
    {
        Debug.Assert(food >= amount);

        food -= amount;
    }

    public void DecreaseWood(int amount)
    {
        Debug.Assert(wood >= amount);

        wood -= amount;
    }

    public void DecreaseGold(int amount)
    {
        Debug.Assert(gold >= amount);

        gold -= amount;
    }
}

using UnityEngine;

public class Farm : CivilianBuilding, ResourceGenerator
{
    private int production = 5;
    public int GetProduction() => production;
    public void GenerateResource(int amount)
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseFood(amount);
    }
}

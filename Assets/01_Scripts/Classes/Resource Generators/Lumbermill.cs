using UnityEngine;

public class Lumbermill : CivilianBuilding, ResourceGenerator
{
    private int production = 5;
    public override int GetProduction() => production;
    public override void GenerateResource(int amount)
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseWood(amount);
    }
}

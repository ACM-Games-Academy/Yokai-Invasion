using UnityEngine;

public class Market : CivilianBuilding, ResourceGenerator
{
    private int production = 5;
    public override int GetProduction() => production;
    public override void GenerateResource(int amount)
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(amount);
    }
}

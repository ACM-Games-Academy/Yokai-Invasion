using UnityEngine;

public class Market : MonoBehaviour, ResourceGenerator
{
    private int production = 5;
    public int GetProduction() => production;
    public void GenerateResource(int amount)
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(amount);
    }
}

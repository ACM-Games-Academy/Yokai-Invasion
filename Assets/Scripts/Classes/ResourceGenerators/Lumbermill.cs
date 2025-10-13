using UnityEngine;

public class Lumbermill : MonoBehaviour, ResourceGenerator
{
    private int production = 5;
    public int GetProduction() => production;
    public void GenerateResource(int amount)
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseWood(amount);
    }
}

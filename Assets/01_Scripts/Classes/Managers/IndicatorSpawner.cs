using UnityEngine;

public class IndicatorSpawner : MonoBehaviour
{

    void Start()
    {
        foreach (var building in settings.BuildingOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(building.buildingPrefab, settings.PoolSize);
        }

        foreach (var building in settings.BuildingOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(building.buildingPrefab, settings.PoolSize);
        }
    }
}


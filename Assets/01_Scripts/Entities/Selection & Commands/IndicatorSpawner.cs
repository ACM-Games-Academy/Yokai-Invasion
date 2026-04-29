using UnityEngine;

public class IndicatorSpawner : MonoBehaviour
{
    private IndicatorSettings settings;
    void Start()
    {
        settings = Overseer.Instance.Settings.IndicatorSettings;

        foreach (var indicator in settings.IndicatorPrefabs)
        {
            Debug.Log(indicator);
            Debug.Log("im literally foing anythigna");
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(indicator, settings.PoolSize);
        }
    }
}


using System.Collections.Generic;
using UnityEngine;
using static BuildingSpawner;

public class UnitSpawner : MonoBehaviour
{
    private UnitSettings settings;

    private int goldCost;
    private int foodCost;

    private GameObject unitSpawnEmpty;
    private Vector3 templeSpawn;

    public Dictionary<string, int> IndexDictionary;
    public List<GameObject> SpawnedUnits = new List<GameObject>();

    private void Awake()
    {
        settings = Overseer.Instance.Settings.UnitSettings;

        unitSpawnEmpty = GameObject.Find("Unit Spawn");
        templeSpawn = unitSpawnEmpty.transform.position;

        IndexDictionary = new Dictionary<string, int>();
        var prefabs = settings.UnitOptions;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (IndexDictionary.ContainsKey(prefabs[i].UnitPrefab.name)) continue;
            IndexDictionary[prefabs[i].UnitPrefab.name] = i;
        }
    }

    private void Start()
    {
        foreach (var unit in settings.UnitOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(unit.UnitPrefab, settings.PoolSize);
        }
    }

    public void SpawnByIndex(int index)
    {
        goldCost = settings.UnitOptions[index].GoldCost;
        foodCost = settings.UnitOptions[index].FoodCost;

        if (!ResourceCheck())
        {
            Debug.LogWarning("Not enough resources to hire this unit!");

            var uiCanvas = Overseer.Instance.GetManager<UiSpawner>().uiCanvas;
            var buildModeInput = uiCanvas.GetComponent<BuildModeInput>();
            StartCoroutine(buildModeInput.TriggerResourcesWarning());

            return;
        }

        PayResources(goldCost, foodCost);

        //get unit from pool
        var spawnedUnit = Overseer.Instance.GetManager<ObjectPooler>().GetPooledObject(
                                                                    settings.UnitOptions[index].UnitPrefab.name,
                                                                    templeSpawn,
                                                                    Quaternion.identity);
    }
    private bool ResourceCheck()
    {
        return (Overseer.Instance.GetManager<ResourceManager>().CurrentGold() >= goldCost &&
            Overseer.Instance.GetManager<ResourceManager>().CurrentFood() >= foodCost);
    }
    private void PayResources(int gold, int food)
    {
        Overseer.Instance.GetManager<ResourceManager>().DecreaseGold(gold);
        Overseer.Instance.GetManager<ResourceManager>().DecreaseFood(food);
    }

}

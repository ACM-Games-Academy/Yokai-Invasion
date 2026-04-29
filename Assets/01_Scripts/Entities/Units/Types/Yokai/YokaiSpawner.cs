using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class YokaiSpawner : MonoBehaviour
{
    private SpawnerSettings settings;

    private List<GameObject> spawnedYokai;

    private void Awake()
    {
        settings = Overseer.Instance.Settings.SpawnerSettings;
    }

    private void Start()
    {
        foreach (var yokai in settings.YokaiPrefabs)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(yokai, settings.PoolSize);
        }
    }

    public GameObject[] SummonHorde(HordeSettings hordeSettings)
    {
        spawnedYokai = new List<GameObject>();
        int points = hordeSettings.PointValue;
        yokaiSpawnOption[] yokaiOptions = hordeSettings.YokaiOptions;

        InitializePoolIfNone(yokaiOptions);

        while (points > 0)
        {
            yokaiSpawnOption[] optionsToRemove = MarkTooExpensiveYokaiToRemove(yokaiOptions, points);
            yokaiOptions = RemoveTooExpensiveYokaiAndRedistributeProbabilities(yokaiOptions, optionsToRemove);
            points = SpawnYokaiFromProbabilities(yokaiOptions, points);
        }
        return spawnedYokai.ToArray();
    }

    private GameObject SpawnYokai(string yokai, Vector3 position, Quaternion rotation)
    {
        var spawnedYokai = Overseer.Instance.GetManager<ObjectPooler>().GetPooledObject(yokai, position, rotation);

        return spawnedYokai;
    }

    private Vector3 RandomSpawnLocation()
    {
        float x = Random.Range(settings.SpawnAreaCenter.x - settings.SpawnAreaSize.x / 2, settings.SpawnAreaCenter.x + settings.SpawnAreaSize.x / 2);
        float z = Random.Range(settings.SpawnAreaCenter.y - settings.SpawnAreaSize.y / 2, settings.SpawnAreaCenter.y + settings.SpawnAreaSize.y / 2);
        return new Vector3(x, settings.SpawnHeight, z);
    }

    private void InitializePoolIfNone(yokaiSpawnOption[] yokaiOptions)
    {
        foreach (var option in yokaiOptions)
        {
            if (Overseer.Instance.GetManager<ObjectPooler>().PoolExists(option.YokaiPrefab.name) == false)
            {
                Overseer.Instance.GetManager<ObjectPooler>().InitializePool(option.YokaiPrefab, 256);
            }
        }
    }

    private yokaiSpawnOption[] MarkTooExpensiveYokaiToRemove(yokaiSpawnOption[] yokaiOptions, int remainingPoints)
    {
        List<yokaiSpawnOption> optionsToRemove = new List<yokaiSpawnOption>();
        foreach (var option in yokaiOptions)
        {
            var optionSettings = option.YokaiPrefab.GetComponent<Yokai>().yokaiSettings;

            if (optionSettings.PointValue > remainingPoints)
            {
                optionsToRemove.Add(option);
            }
        }

        return optionsToRemove.ToArray();
    }

    private yokaiSpawnOption[] RemoveTooExpensiveYokaiAndRedistributeProbabilities(yokaiSpawnOption[] yokaiOptions, yokaiSpawnOption[] optionsToRemove)
    {
        foreach (var option in optionsToRemove)
        {
            float probabilityToDistribute = option.SpawnProbability;
            var newOptions = new List<yokaiSpawnOption>(yokaiOptions);
            newOptions.Remove(option);
            var probabilityPerOption = probabilityToDistribute / newOptions.Count;

            for (int i = 0; i < newOptions.Count; i++)
            {
                var newOption = newOptions[i];
                newOption.SpawnProbability += probabilityPerOption;
            }

            yokaiOptions = newOptions.ToArray();
        }
        return yokaiOptions;
    }

    //yokaiSpawnOptions is an array of structs (set in HordeSettings), option is a single struct from the array
    private int SpawnYokaiFromProbabilities(yokaiSpawnOption[] yokaiOptions, int points)
    {
        float roll = Random.Range(0f, 100f);
        for (int i = 0; i < yokaiOptions.Length; i++)
        {
            var option = yokaiOptions[i];
            if (roll <= option.SpawnProbability)
            {
                var optionSettings = option.YokaiPrefab.GetComponent<Yokai>().yokaiSettings;
                var newYokai = SpawnYokai(option.YokaiPrefab.name, RandomSpawnLocation(), Quaternion.identity);
                spawnedYokai.Add(newYokai);
                points -= optionSettings.PointValue;
                break;
            }
            else
            {
                roll -= option.SpawnProbability;
            }
        }
        return points;
    }

    public void DespawnHorde()
    {
        foreach (GameObject yokai in spawnedYokai)
        {
            Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(yokai);
        }
    }
}

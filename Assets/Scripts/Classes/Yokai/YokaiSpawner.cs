using UnityEngine;
using System.Collections.Generic;

public class YokaiSpawner : MonoBehaviour
{
    private SpawnerSettings settings;

    private void Start()
    {
        foreach (var yokai in settings.YokaiPrefabs)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(yokai, settings.PoolSize);
        }
    }

    public GameObject[] SummonHorde(HordeSettings hordeSettings)
    {
        int points = hordeSettings.PointValue;
        yokaiSpawnOption[] yokaiOptions = hordeSettings.YokaiOptions;
        var spawnedYokai = new List<GameObject>();
        var optionsToRemove = new List<yokaiSpawnOption>();

        foreach (var option in yokaiOptions)
        {
            if (objectPooler.PoolExists(option.yokaiPrefab.name) == false)
            {
                objectPooler.InitializePool(option.yokaiPrefab, 256);
            }
        }

        while (points > 0)
        {
            float roll = Random.Range(0f, 100f);

            foreach (var option in yokaiOptions)
            {
                var optionSettings = option.yokaiPrefab.GetComponent<Yokai>().yokaiSettings;

                if (optionSettings.PointValue > points)
                {
                    optionsToRemove.Add(option);
                }
            }
            foreach (var option in optionsToRemove)
            {
                float probabilityToDistribute = option.spawnProbability;
                var newOptions = new List<yokaiSpawnOption>(yokaiOptions);
                newOptions.Remove(option);
                var probabilityPerOption = probabilityToDistribute / newOptions.Count;

                for (int i = 0; i < newOptions.Count; i++)
                {
                    var newOption = newOptions[i];
                    newOption.spawnProbability += probabilityPerOption;
                }

                yokaiOptions = newOptions.ToArray();
            }
            optionsToRemove.Clear();

            for (int i = 0; i < yokaiOptions.Length; i++)
            {
                var option = yokaiOptions[i];
                if (roll <= option.spawnProbability)
                {
                    var optionSettings = option.yokaiPrefab.GetComponent<Yokai>().yokaiSettings;
                    var newYokai = SpawnYokai(option.yokaiPrefab.name, RandomSpawnLocation(), Quaternion.identity);
                    spawnedYokai.Add(newYokai);
                    points -= optionSettings.PointValue;
                    break;
                }
                else
                {
                    roll -= option.spawnProbability;
                }
            }
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

    public void SetSettings(SpawnerSettings newSettings)
    {
        settings = newSettings;
    }
}

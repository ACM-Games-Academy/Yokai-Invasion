using UnityEngine;
using System.Collections.Generic;

public class YokaiSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] yokaiPrefabs;

    [SerializeField] private Transform templePosition;
    [SerializeField] private Boids boidScript;
    [SerializeField] private AStar aStarScript;

    [SerializeField] private Vector2 spawnAreaCenter;
    [SerializeField] private Vector2 spawnAreaSize;
    [SerializeField] private float spawnHeight;

    private void Start()
    {
        foreach (var yokai in yokaiPrefabs)
        {
            ObjectPooler.Instance.InitializePool(yokai, 256);
        }
    }

    public GameObject[] SummonHorde(HordeSettings hordeSettings)
    {
        int points = hordeSettings.PointValue;
        yokaiSpawnOption[] yokaiOptions = hordeSettings.YokaiOptions;
        var spawnedYokai = new List<GameObject>();
        var optionsToRemove = new List<yokaiSpawnOption>();

        while (points > 0)
        {
            float roll = Random.Range(0f, 100f);

            foreach (var option in yokaiOptions)
            {
                var optionSettings = option.yokaiPrefab.GetComponent<IYokai>().yokaiSettings;

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
                    var optionSettings = option.yokaiPrefab.GetComponent<IYokai>().yokaiSettings;
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
        var spawnedYokai = ObjectPooler.Instance.GetPooledObject(yokai, position, rotation);
        var yokaiPathing = spawnedYokai.GetComponent<YokaiPathing>();

        yokaiPathing.SetTempleLocation(templePosition);
        yokaiPathing.SetBoidScript(boidScript);
        yokaiPathing.SetAStarScript(aStarScript);

        return spawnedYokai;
    }

    private Vector3 RandomSpawnLocation()
    {
        float x = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float z = Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2);
        return new Vector3(x, spawnHeight, z);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class YokaiManager : MonoBehaviour
{
    private GameObject[] yokaiGameObjects = new GameObject[0];

    private YokaiSpawner yokaiSpawner;

    private HordeSettings[] hordeSettings;
    private SpawnerSettings spawnerSettings;

    private void Start()
    {
        var yokaiSpawnerType = typeof(YokaiSpawner);

        GameObject yokaiSpawnerObject = new GameObject(yokaiSpawnerType.Name);
        yokaiSpawnerObject.transform.SetParent(transform);

        yokaiSpawner = yokaiSpawnerObject.AddComponent<YokaiSpawner>();
        yokaiSpawner.SetSettings(spawnerSettings);

        StartCoroutine(DelayedHordeSummon(hordeSettings[0], 1f));
    }

    private IEnumerator DelayedHordeSummon(HordeSettings hordeSettings, float delay)
    {
        yield return new WaitForSeconds(delay);
        yokaiSpawner.SummonHorde(hordeSettings);
    }

    public void SetHordeSettings(HordeSettings[] newHordeSettings)
    {
        hordeSettings = newHordeSettings;
    }

    public void SetSpawnerSettings(SpawnerSettings newSpawnerSettings)
    {
        spawnerSettings = newSpawnerSettings;
    }
}

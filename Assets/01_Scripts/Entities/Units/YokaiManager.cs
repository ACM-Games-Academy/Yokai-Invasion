using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class YokaiManager : MonoBehaviour
{
    private YokaiSpawner yokaiSpawner;
    private HordeSettings[] hordes;

    private int nightCount;

    private void Start()
    {
        nightCount = 0;
        hordes = Overseer.Instance.Settings.HordeSettings;

        Overseer.Instance.GetManager<NightCycle>().NightStarted += BeginNight;
        Overseer.Instance.GetManager<NightCycle>().NightEnded += EndNight;

        var yokaiSpawnerType = typeof(YokaiSpawner);

        GameObject yokaiSpawnerObject = new GameObject(yokaiSpawnerType.Name);
        yokaiSpawnerObject.transform.SetParent(transform);

        yokaiSpawner = yokaiSpawnerObject.AddComponent<YokaiSpawner>();
    }

    private void BeginNight()
    {
        if (nightCount >= hordes.Length)
        {
            nightCount = hordes.Length - 1;
        }
        yokaiSpawner.SummonHorde(hordes[nightCount]);
        nightCount++;
    }
    private void EndNight()
    {
        yokaiSpawner.DespawnHorde();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class YokaiManager : MonoBehaviour
{
    private YokaiSpawner yokaiSpawner;
    private HordeSettings[] hordes;

    private void Start()
    {
        hordes = Overseer.Instance.Settings.HordeSettings;

        Overseer.Instance.GetManager<NightCycle>().NightStarted += BeginNight;

        var yokaiSpawnerType = typeof(YokaiSpawner);

        GameObject yokaiSpawnerObject = new GameObject(yokaiSpawnerType.Name);
        yokaiSpawnerObject.transform.SetParent(transform);

        yokaiSpawner = yokaiSpawnerObject.AddComponent<YokaiSpawner>();
    }

    private void BeginNight()
    {
        yokaiSpawner.SummonHorde(hordes[0]);
    }
}

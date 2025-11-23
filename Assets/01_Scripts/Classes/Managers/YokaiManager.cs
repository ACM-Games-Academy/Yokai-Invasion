using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class YokaiManager : MonoBehaviour
{
    private YokaiSpawner yokaiSpawner;
    private NightSettings[] nights;

    private byte currentNightIndex = 0;
    private byte currentHordeIndex = 0;
    private float waveWaitTime;

    private void Start()
    {
        nights = Overseer.Instance.Settings.NightSettings;

        Overseer.Instance.GetManager<NightCycle>().NightStarted += BeginNight;

        var yokaiSpawnerType = typeof(YokaiSpawner);

        GameObject yokaiSpawnerObject = new GameObject(yokaiSpawnerType.Name);
        yokaiSpawnerObject.transform.SetParent(transform);

        yokaiSpawner = yokaiSpawnerObject.AddComponent<YokaiSpawner>();

        Overseer.Instance.GetManager<NightCycle>().NightStarted += BeginNight;
    }

    private void BeginNight()
    {
        waveWaitTime = Overseer.Instance.Settings.NightCycleSettings.NightLengthSeconds / (nights[currentNightIndex].Hordes.Length + 1);
        StartCoroutine(SpawnHorde(nights[currentNightIndex].Hordes[currentHordeIndex]));
    }

    private IEnumerator SpawnHorde(HordeSettings hordeSettings)
    {
        GameObject[] activeYokai = yokaiSpawner.SummonHorde(hordeSettings);
        float timer = 0f;

        while (activeYokai.Any(yokai => yokai.activeInHierarchy) && timer < waveWaitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        currentHordeIndex++;
        if (currentHordeIndex >= nights[currentNightIndex].Hordes.Length)
        {
            currentHordeIndex = 0;
            currentNightIndex++;
            if (currentNightIndex >= nights.Length)
            {
                currentNightIndex = 0;
            }
            yield break;
        }
        yield return StartCoroutine(SpawnHorde(nights[currentNightIndex].Hordes[currentHordeIndex]));
    }
}

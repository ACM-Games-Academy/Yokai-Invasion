using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NightCycle : MonoBehaviour
{
    private uint currentCycle = 0;

    private NightCycleSettings settings;
    public Action NightStarted;
    public Action NightEnded;

    public enum TimeOfDay
    {
        Dawn,
        Day,
        Dusk,
        Night
    }
    public TimeOfDay currentTimeOfDay = TimeOfDay.Day;

    void Start()
    {
        settings = Overseer.Instance.Settings.NightCycleSettings;
        StartCoroutine(StartDawn());
    }

    private IEnumerator StartDawn()
    {
        Debug.Log($"It is Dawn {currentCycle}");

        currentTimeOfDay = TimeOfDay.Dawn;
        yield return new WaitForSeconds(settings.DawnLengthSeconds);
        StartCoroutine(StartDay());
    }

    private IEnumerator StartDay()
    {
        Debug.Log($"It is Day {currentCycle}");

        currentTimeOfDay = TimeOfDay.Day;
        yield return new WaitForSeconds(settings.DayLengthSeconds);
        StartCoroutine(StartDusk());
    }

    private IEnumerator StartDusk()
    {
        Debug.Log($"It is Dusk {currentCycle}");

        currentTimeOfDay = TimeOfDay.Dusk;
        yield return new WaitForSeconds(settings.DuskLengthSeconds);
        StartCoroutine(StartNight());
    }

    private IEnumerator StartNight()
    {
        Debug.Log($"It is Night {currentCycle}");

        currentTimeOfDay = TimeOfDay.Night;
        NightStarted?.Invoke();
        yield return new WaitForSeconds(settings.NightLengthSeconds);
        NightEnded?.Invoke();
        currentCycle++;
        StartCoroutine(StartDawn());
    }
}

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NightCycle : MonoBehaviour
{
    private uint currentCycle = 0;

    private NightCycleSettings settings;

    public Action DawnStarted;
    public Action DayStarted;
    public Action DuskStarted;
    public Action NightStarted;

    public enum TimeOfDay
    {
        Dawn,
        Day,
        Dusk,
        Night
    }
    public TimeOfDay CurrentTimeOfDay = TimeOfDay.Day;

    void Start()
    {
        settings = Overseer.Instance.Settings.NightCycleSettings;
        StartCoroutine(StartDawn());
    }

    private IEnumerator StartDawn()
    {
        //Debug.Log($"It is Dawn {currentCycle + 1}");

        CurrentTimeOfDay = TimeOfDay.Dawn;
        DawnStarted?.Invoke();
        yield return new WaitForSeconds(settings.DawnLengthSeconds);
        StartCoroutine(StartDay());
    }

    private IEnumerator StartDay()
    {
        //Debug.Log($"It is Day {currentCycle + 1}");

        CurrentTimeOfDay = TimeOfDay.Day;
        DayStarted?.Invoke();
        yield return new WaitForSeconds(settings.DayLengthSeconds);
        StartCoroutine(StartDusk());
    }

    private IEnumerator StartDusk()
    {
        //Debug.Log($"It is Dusk {currentCycle + 1}");

        CurrentTimeOfDay = TimeOfDay.Dusk;
        DuskStarted?.Invoke();
        yield return new WaitForSeconds(settings.DuskLengthSeconds);
        StartCoroutine(StartNight());
    }

    private IEnumerator StartNight()
    {
        //Debug.Log($"It is Night {currentCycle + 1}");

        CurrentTimeOfDay = TimeOfDay.Night;
        NightStarted?.Invoke();
        yield return new WaitForSeconds(settings.NightLengthSeconds);
        currentCycle++;
        StartCoroutine(StartDawn());
    }
}

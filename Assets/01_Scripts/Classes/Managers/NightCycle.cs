using System.Collections;
using UnityEngine;

public class NightCycle : MonoBehaviour
{
    //    private uint currentCycle = 0;

    //    public enum TimeOfDay
    //    {
    //        Dawn,
    //        Day,
    //        Twilight,
    //        Night
    //    }
    //    public TimeOfDay currentTimeOfDay = TimeOfDay.Day;

    //    void Start()
    //    {
    //        StartDawn(currentCycle);
    //    }

    //    private IEnumerator StartDawn(uint cycle)
    //    {   
    //        currentTimeOfDay = TimeOfDay.Dawn;
    //        yield return new WaitForSeconds(dawnLengthSeconds);
    //        StartCoroutine(StartDay(cycle));
    //    }

    //    private IEnumerator StartDay(uint cycle)
    //    {
    //        currentTimeOfDay = TimeOfDay.Day;
    //        yield return new WaitForSeconds(dayLengthSeconds);
    //        StartCoroutine(StartTwilight(cycle));
    //    }

    //    private IEnumerator StartTwilight(uint cycle)
    //    {
    //        currentTimeOfDay = TimeOfDay.Twilight;
    //        yield return new WaitForSeconds(twilightLengthSeconds);
    //        StartCoroutine(StartNight(cycle));
    //    }

    //    private IEnumerator StartNight(uint cycle)
    //    {
    //        currentTimeOfDay = TimeOfDay.Night;
    //        yield return new WaitForSeconds(nightLengthSeconds);
    //        StartCoroutine(StartDawn(cycle++));
    //    }
}

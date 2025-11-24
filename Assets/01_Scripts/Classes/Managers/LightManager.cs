using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject dawnLight;
    [SerializeField] private GameObject dayLight;
    [SerializeField] private GameObject duskLight;
    [SerializeField] private GameObject nightLight;

    void Start()
    {
        var nightCycle = Overseer.Instance.GetManager<NightCycle>();
        nightCycle.DawnStarted += ActivateDawnLighting;
        nightCycle.DayStarted += ActivateDayLighting;
        nightCycle.DuskStarted += ActivateDuskLighting;
        nightCycle.NightStarted += ActivateNightLighting;
    }

    private void ActivateDawnLighting()
    {
        BecomeLightless();
        dawnLight.SetActive(true);
    }
    private void ActivateDayLighting()
    {
        BecomeLightless();
        dayLight.SetActive(true);
    }
    private void ActivateDuskLighting()
    {
        BecomeLightless();
        duskLight.SetActive(true);
    }
    private void ActivateNightLighting()
    {
        BecomeLightless();
        nightLight.SetActive(true);
    }

    private void BecomeLightless()
    {
        dawnLight.SetActive(false);
        dayLight.SetActive(false);
        duskLight.SetActive(false);
        nightLight.SetActive(false);
    }
}

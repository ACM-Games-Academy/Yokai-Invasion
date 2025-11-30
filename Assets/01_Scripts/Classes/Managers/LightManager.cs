using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Light;

    public Animator dayNNite;

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
        dayNNite.SetTrigger("Night-Dawn");
    }
    private void ActivateDayLighting()
    {
        dayNNite.SetTrigger("Dawn-Day");
    }
    private void ActivateDuskLighting()
    {
        dayNNite.SetTrigger("Day-Dusk");
    }
    private void ActivateNightLighting()
    {
        dayNNite.SetTrigger("Dusk-Night");
    }
}

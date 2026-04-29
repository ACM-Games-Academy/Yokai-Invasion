using UnityEngine;

[CreateAssetMenu(fileName = "NightCycleSettings", menuName = "ScriptableObjects/NightCycleSettings")]
public class NightCycleSettings : ScriptableObject
{
    [Range(0,300)]
    [SerializeField]
    private int dawnLengthSeconds;
    [Range(0, 300)]
    [SerializeField]
    private int dayLengthSeconds;
    [Range(0, 300)]
    [SerializeField]
    private int duskLengthSeconds;
    [Range(0, 300)]
    [SerializeField]
    private int nightLengthSeconds;

    public int DawnLengthSeconds => dawnLengthSeconds;
    public int DayLengthSeconds => dayLengthSeconds;
    public int DuskLengthSeconds => duskLengthSeconds;
    public int NightLengthSeconds => nightLengthSeconds;
}

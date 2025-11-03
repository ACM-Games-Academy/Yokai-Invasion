using UnityEngine;

[CreateAssetMenu(fileName = "NightCycleSettings", menuName = "ScriptableObjects/NightCycleSettings")]
public class NightCycleSettings : ScriptableObject
{
    private uint dawnLengthSeconds;
    private uint dayLengthSeconds;
    private uint twilightLengthSeconds;
    private uint nightLengthSeconds;

    public uint DawnLengthSettings => dawnLengthSeconds;
    public uint DayLengthSettings => dayLengthSeconds;
    public uint TwilightLengthSettings => twilightLengthSeconds;
    public uint NightLengthSettings => nightLengthSeconds;
}

using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Scriptable Objects/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    [SerializeField]
    private AK.Wwise.Event[] events;
    [SerializeField]
    private AK.Wwise.RTPC[] rtpcs;
    [SerializeField]
    private AK.Wwise.State[] states;

    public AK.Wwise.Event[] Events => events;

    public AK.Wwise.RTPC[] RTPCs => rtpcs;

    public AK.Wwise.State[] States => states;
}

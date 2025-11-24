using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Scriptable Objects/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    [SerializeField]
    private AK.Wwise.Event[] events;
    [SerializeField]
    private AK.Wwise.RTPC[] rtpcs;

    public AK.Wwise.Event[] Events => events;

    public AK.Wwise.RTPC[] RTPCs => rtpcs;
}

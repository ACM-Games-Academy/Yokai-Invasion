using UnityEngine;

[CreateAssetMenu(fileName = "NightSettings", menuName = "ScriptableObjects/NightSettings")]
public class NightSettings : ScriptableObject
{
    [SerializeField]
    private HordeSettings[] hordes;
    public HordeSettings[] Hordes => hordes;

    private void OnValidate()
    {
        if (hordes == null || hordes.Length == 0)
        {
            Debug.LogError($"{name} is a night with no assigned hordes.");
        }
    }
}

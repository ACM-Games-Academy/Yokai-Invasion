using UnityEngine;

[CreateAssetMenu(fileName = "OverseerSettings", menuName = "ScriptableObjects/OverseerSettings")]
public class OverseerSettings : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private HordeSettings[] hordeSettings;
    [SerializeField]
    private SpawnerSettings spawnerSettings;


    public HordeSettings[] HordeSettings => hordeSettings;
    public SpawnerSettings SpawnerSettings => spawnerSettings;
}

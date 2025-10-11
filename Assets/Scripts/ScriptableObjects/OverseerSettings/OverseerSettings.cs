using UnityEngine;

[CreateAssetMenu(fileName = "OverseerSettings", menuName = "ScriptableObjects/OverseerSettings")]
public class OverseerSettings : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private HordeSettings[] hordeSettings;
    [SerializeField]
    private SpawnerSettings spawnerSettings;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject selectionCanvas;

    public HordeSettings[] HordeSettings => hordeSettings;
    public SpawnerSettings SpawnerSettings => spawnerSettings;
    public GameObject SelectionCanvas => selectionCanvas;
}

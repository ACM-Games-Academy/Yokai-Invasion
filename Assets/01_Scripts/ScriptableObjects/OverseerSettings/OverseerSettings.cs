using UnityEngine;

[CreateAssetMenu(fileName = "OverseerSettings", menuName = "ScriptableObjects/OverseerSettings")]
public class OverseerSettings : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private HordeSettings[] hordeSettings;
    [SerializeField]
    private SpawnerSettings spawnerSettings;
    [SerializeField]
    private BuildingSettings buildingSettings;
    [SerializeField]
    private NightCycleSettings nightCycleSettings;
    [SerializeField]
    private IndicatorSettings indicatorSettings;
    [SerializeField]
    private AudioSettings audioSettings;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject selectionCanvas;

    public HordeSettings[] HordeSettings => hordeSettings;
    public SpawnerSettings SpawnerSettings => spawnerSettings;
    public BuildingSettings BuildingSettings => buildingSettings;
    public GameObject SelectionCanvas => selectionCanvas;
    public NightCycleSettings NightCycleSettings => nightCycleSettings;
    public IndicatorSettings IndicatorSettings => indicatorSettings;

    public AudioSettings AudioSettings => audioSettings;
}

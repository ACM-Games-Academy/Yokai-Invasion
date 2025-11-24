using UnityEngine;

[CreateAssetMenu(fileName = "OverseerSettings", menuName = "ScriptableObjects/OverseerSettings")]
public class OverseerSettings : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private NightSettings[] nightSettings;
    [SerializeField]
    private SpawnerSettings spawnerSettings;
    [SerializeField]
    private BuildingSettings buildingSettings;
    [SerializeField]
    private NightCycleSettings nightCycleSettings;
    [SerializeField]
    private IndicatorSettings indicatorSettings;
    [SerializeField]
    private UnitSettings unitSettings;
    [SerializeField]
    private AudioSettings audioSettings;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject selectionCanvas;
    [SerializeField]
    private GameObject uiCanvas;

    public NightSettings[] NightSettings => nightSettings;
    public SpawnerSettings SpawnerSettings => spawnerSettings;
    public BuildingSettings BuildingSettings => buildingSettings;
    public GameObject SelectionCanvas => selectionCanvas;
    public NightCycleSettings NightCycleSettings => nightCycleSettings;
    public IndicatorSettings IndicatorSettings => indicatorSettings;
    public UnitSettings UnitSettings => unitSettings;
    public GameObject UiCanvas => uiCanvas;
    public AudioSettings AudioSettings => audioSettings;
}

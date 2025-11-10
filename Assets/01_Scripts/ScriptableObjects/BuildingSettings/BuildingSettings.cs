using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSettings", menuName = "ScriptableObjects/BuildingSettings")]
public class BuildingSettings : ScriptableObject
{
    [SerializeField] private int poolSize;
    [SerializeField] private float spawnHeight;

    public int PoolSize => poolSize;
    public float SpawnHeight => spawnHeight;

    [Tooltip("All available Building Types in Build Menu.")]
    [SerializeField]
    private buildingSpawnOption[] buildingOptions;

    public buildingSpawnOption[] BuildingOptions => buildingOptions;





    [Serializable]
    public struct buildingSpawnOption
    {
        [Tooltip("The Building prefab to spawn.")]
        public GameObject BuildingPrefab;
        [Tooltip("The wood cost of the building, from 0 to 100.")]
        [Range(0, 100)]
        public int WoodCost;
        [Tooltip("The gold cost of the building, from 0 to 100.")]
        [Range(0, 100)]
        public int GoldCost;
        [Tooltip("The total health of this building type.")]
        [Range(0, 1000)]
        public int BuildingHealth;
    }
}

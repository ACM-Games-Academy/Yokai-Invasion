using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSettings", menuName = "ScriptableObjects/UnitSettings")]
public class UnitSettings : ScriptableObject
{
    [SerializeField]
    private float waypointTolerance;
    [SerializeField]
    private float moveSpeed;

    public float WaypointTolerance => waypointTolerance;
    public float MoveSpeed => moveSpeed;

    [SerializeField] private int poolSize;

    [Tooltip("All available Unit Types in Hiring Menu.")]
    [SerializeField]
    private unitSpawnOption[] unitOptions;

    public unitSpawnOption[] UnitOptions => unitOptions;
    public int PoolSize => poolSize;

    [Serializable]
    public struct unitSpawnOption
    {
        [Tooltip("The Unit prefab to spawn.")]
        public GameObject UnitPrefab;
        [Tooltip("The food cost of the unit, from 0 to 100.")]
        [Range(0, 100)]
        public int FoodCost;
        [Tooltip("The gold cost of the unit, from 0 to 100.")]
        [Range(0, 100)]
        public int GoldCost;
    }
}

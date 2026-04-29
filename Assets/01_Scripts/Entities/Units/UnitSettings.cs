using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSettings", menuName = "ScriptableObjects/UnitSettings")]
public class UnitSettings : ScriptableObject
{
    [SerializeField]
    private float waypointTolerance;
    [SerializeField]
    private float moveSpeed;
    [SerializeField] 
    private int poolSize;

    [Header ("Ashigaru Combat Settings")]
    [SerializeField]
    private int totalHealth;
    [Range(0, 50)]
    [SerializeField]
    private int attackPower;
    [Range(0, 25)]
    [SerializeField]
    private float attackRange;
    [Range(0, 10)]
    [SerializeField]
    private float attackDelay;

    public float WaypointTolerance => waypointTolerance;
    public float MoveSpeed => moveSpeed;
    public int TotalHealth => totalHealth;
    public int AttackPower => attackPower;
    public float AttackRange => attackRange;
    public float AttackDelay => attackDelay;


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

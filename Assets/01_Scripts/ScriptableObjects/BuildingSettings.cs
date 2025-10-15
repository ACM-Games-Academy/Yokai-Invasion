using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSettings", menuName = "Scriptable Objects/BuildingSettings")]
public class BuildingSettings : ScriptableObject
{
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private int poolSize;
    [SerializeField] private float spawnHeight;


    public GameObject[] TowerPrefabs => towerPrefabs;
    public int PoolSize => poolSize;
    public float SpawnHeight => spawnHeight;
}

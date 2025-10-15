using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerSettings", menuName = "ScriptableObjects/SpawnerSettings")]
public class SpawnerSettings : ScriptableObject
{
    [SerializeField] private GameObject[] yokaiPrefabs;

    [SerializeField] private Vector2 spawnAreaCenter;
    [SerializeField] private Vector2 spawnAreaSize;
    [SerializeField] private float spawnHeight;
    [SerializeField] private int poolSize;

    public GameObject[] YokaiPrefabs => yokaiPrefabs;
    public Vector2 SpawnAreaCenter => spawnAreaCenter;
    public Vector2 SpawnAreaSize => spawnAreaSize;
    public float SpawnHeight => spawnHeight;
    public int PoolSize => poolSize;
}

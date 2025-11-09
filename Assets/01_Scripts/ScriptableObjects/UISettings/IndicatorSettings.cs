using UnityEngine;

[CreateAssetMenu(fileName = "IndicatorSettings", menuName = "ScriptableObjects/IndicatorSettings")]
public class IndicatorSettings : ScriptableObject
{
    [SerializeField] private GameObject[] indicatorPrefabs;
    [SerializeField] private int poolSize;
    public GameObject[] IndicatorPrefabs => indicatorPrefabs;
    public int PoolSize => poolSize;
}

using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HordeSettings", menuName = "ScriptableObjects/HordeSettings")]
public class HordeSettings : ScriptableObject
{
    [Header("Horde Settings")]
    [Tooltip("The number of points used to buy Yokai for this horde.")]
    [Range(1, 100)]
    [SerializeField]
    private int pointValue;

    [Tooltip("The available Yokai for this horde and the probability of spawning them, probabilities ought to sum to 100.")]
    [SerializeField]
    private yokaiSpawnOption[] yokaiOptions;

    public int PointValue => pointValue;
    public yokaiSpawnOption[] YokaiOptions => yokaiOptions;

    private void OnValidate()
    {
        float total = yokaiOptions.Sum(opt => opt.spawnProbability);

        if (Math.Abs(total - 100f) > 0.01f)
        {
            Debug.LogError($"HordeSettings '{name}' has probabilities that sum to {total:F2}% (should be 100%).");
        }
    }
}

[Serializable]
public struct yokaiSpawnOption
{
    [Tooltip("The Yokai prefab to spawn.")]
    public GameObject yokaiPrefab;
    [Tooltip("The probability of this Yokai spawning, from 0 to 100.")]
    [Range(0f, 100f)]
    public float spawnProbability;
}
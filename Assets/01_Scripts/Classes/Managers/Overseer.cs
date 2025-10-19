using System;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// Creates all other manager objects and attaches their scripts.
/// Creates the singleton instance reference to access managers easily.
/// </summary>
public class Overseer : MonoBehaviour
{
    [SerializeField] private OverseerSettings settings;
    public OverseerSettings Settings { get { return settings; } }

    public static Overseer Instance { get; private set; }

    private Dictionary<Type, MonoBehaviour> managerRegistry = new();

    private Type[] managerTypes = new Type[]
    {
        typeof(YokaiManager),
        typeof(ObjectPooler),
        typeof(SelectionManager),
        typeof(ResourceManager),
        typeof(BuildingSpawner),
        typeof(BuildModeInput)
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // DontDestroyOnLoad(gameObject);
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        foreach (Type type in managerTypes)
        {
            GameObject obj = new GameObject(type.Name);
            obj.transform.SetParent(transform);

            MonoBehaviour manager = obj.AddComponent(type) as MonoBehaviour;
            if (manager == null)
            {
                Debug.LogError($"Failed to add {type.Name} as MonoBehaviour.");
                continue;
            }

            managerRegistry[type] = manager;
        }
    }

    public T GetManager<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (managerRegistry.TryGetValue(type, out MonoBehaviour manager))
        {
            return manager as T;
        }

        Debug.LogError($"Manager of type {type.Name} not found.");
        return null;
    }
}

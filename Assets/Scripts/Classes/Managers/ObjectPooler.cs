using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Transform poolParent;

    public static ObjectPooler Instance { get; private set; }

    private void Awake()
    {
        poolParent = new GameObject("Object Pools").transform;
        poolParent.SetParent(transform);
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializePool(GameObject prefab, int poolSize)
    {
        string key = prefab.name;

        if (poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Pool for {key} already exists!");
            return;
        }

        Queue<GameObject> objectPool = new Queue<GameObject>();
        GameObject parent = new GameObject($"{key} Pool");
        parent.transform.SetParent(poolParent);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, parent.transform);
            obj.name = key; // strip "(Clone)" for consistency
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary[key] = objectPool;
    }

    public GameObject GetPooledObject(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"No pool found for key: {key}");
            return null;
        }

        var pool = poolDictionary[key];
        if (pool.Count == 0)
        {
            Debug.LogWarning($"Pool for {key} is empty!");
            return null;
        }

        GameObject obj = pool.Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void ReturnPooledObject(GameObject obj)
    {
        string key = obj.name;
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"No pool found for key: {key}");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }

    public bool PoolExists(string key)
    {
        return poolDictionary.ContainsKey(key);
    }
}

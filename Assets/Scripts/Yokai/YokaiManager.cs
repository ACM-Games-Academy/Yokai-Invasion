using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YokaiManager : MonoBehaviour
{
    [SerializeField]
    private Transform templePosition;
    [SerializeField]
    private GameObject yokaiPrefab;
    [SerializeField]
    private Vector3[] yokaiSpawnPoints;
    private GameObject[] yokaiGameObjects = new GameObject[0];
    private Transform[] yokaiTransforms = new Transform[0];

    public void SpawnYokai(Vector3[] spawnPoints, GameObject yokaiPrefab, int numberToSpawn)
    {
        List<Transform> newYokaiTransforms = new List<Transform>();
        List<GameObject> newYokaiGameObjects = new List<GameObject>();

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject newYokai = Instantiate(yokaiPrefab, spawnPoints[i], Quaternion.LookRotation(Vector3.up, Vector3.up));
            newYokai.GetComponent<YokaiPathing>().SetTempleLocation(templePosition);

            newYokaiTransforms.Add(newYokai.transform);
            newYokaiGameObjects.Add(newYokai);
        }

        yokaiTransforms = yokaiTransforms.Concat(newYokaiTransforms).ToArray();
        yokaiGameObjects = yokaiGameObjects.Concat(newYokaiGameObjects).ToArray();
    }

    private void Start()
    {
        SpawnYokai(yokaiSpawnPoints, yokaiPrefab, yokaiSpawnPoints.Length);
    }
}

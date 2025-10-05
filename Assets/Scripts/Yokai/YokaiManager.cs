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
    private Boids boidScript;
    [SerializeField]
    private AStar aStarScript;
    [SerializeField]
    private int numberOfYokaiToSpawn;
    [SerializeField]
    private float spawnMinVal = -50f;
    [SerializeField]
    private float spawnMaxVal = 50f;
    private Vector3[] yokaiSpawnPoints;
    private GameObject[] yokaiGameObjects = new GameObject[0];
    private Transform[] yokaiTransforms = new Transform[0];

    [SerializeField]
    private Vector3 boidWeights; // x = alignment, y = cohesion, z = separation

    public void SpawnYokai(Vector3[] spawnPoints, GameObject yokaiPrefab, int numberToSpawn, Vector3 boidWeights)
    {
        List<Transform> newYokaiTransforms = new List<Transform>();
        List<GameObject> newYokaiGameObjects = new List<GameObject>();

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject newYokai = Instantiate(yokaiPrefab, spawnPoints[i], Quaternion.LookRotation(Vector3.up, Vector3.up));
            var newYokaiScript = newYokai.GetComponent<YokaiPathing>();

            newYokaiScript.SetTempleLocation(templePosition);
            newYokaiScript.SetBoidWeights(boidWeights);
            newYokaiScript.SetBoidScript(boidScript);
            newYokaiScript.SetAStarScript(aStarScript);

            newYokaiTransforms.Add(newYokai.transform);
            newYokaiGameObjects.Add(newYokai);
        }

        yokaiTransforms = yokaiTransforms.Concat(newYokaiTransforms).ToArray();
        yokaiGameObjects = yokaiGameObjects.Concat(newYokaiGameObjects).ToArray();
    }

    private void Start()
    {
        SpawnYokai(GenerateRandomSpawnPoints(numberOfYokaiToSpawn, spawnMinVal, spawnMaxVal), yokaiPrefab, numberOfYokaiToSpawn, boidWeights);
    }

    private Vector3[] GenerateRandomSpawnPoints(int numberOfPoints, float minVal, float maxVal)
    {
        Vector3[] spawnPoints = new Vector3[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = Random.Range(minVal, maxVal);
            float z = Random.Range(minVal, maxVal);
            spawnPoints[i] = new Vector3(x, 0.66f, z);
        }
        return spawnPoints;
    }
}

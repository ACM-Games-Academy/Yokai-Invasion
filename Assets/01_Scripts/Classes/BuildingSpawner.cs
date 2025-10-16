using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSpawner : MonoBehaviour
{
    private BuildingSettings settings;

    private List<GameObject> spawnedBuildings;

    private Vector3 spawnLocation;


    private void Awake()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;
    }

    private void Start()
    {
        //on Start make object pool of buildings
        foreach (var building in settings.BuildingOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(building.buildingPrefab, settings.PoolSize);
        }

    }


    //Get building from object pool
    private GameObject SpawnBuilding(string key, Vector3 position, Quaternion rotation)
    {
        var spawnedBuilding = Overseer.Instance.GetManager<ObjectPooler>().GetPooledObject(key, position, rotation);

        return spawnedBuilding;
    }

    //set Spawn Location of Building
    private Vector3 SetSpawnLocation()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo))
        {
            spawnLocation = hitInfo.point;
            Debug.Log("Spawn Tower at " + spawnLocation);
        }
        return spawnLocation;
    }

    public void SpawnAtIndex(int index)
    {
        SpawnBuilding(settings.BuildingOptions[index].buildingPrefab.name, SetSpawnLocation(), Quaternion.identity);
    }
}

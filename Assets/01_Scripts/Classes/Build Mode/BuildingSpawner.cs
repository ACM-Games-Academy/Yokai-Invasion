using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSpawner : MonoBehaviour
{
    private BuildingSettings settings;

    public List<GameObject> spawnedBuildings = new List<GameObject>();
    private Vector3 spawnLocation;

    public bool buildingHasBeenSpawned = false;


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

    private void Update()
    {
        if (buildingHasBeenSpawned)
        {
            MoveBuildingToCursor();
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
        }
        return spawnLocation;
    }

    //Spawn the type of building based on index (called in ui manager)
    public void SpawnAtIndex(int index)
    {
        var spawnedBuilding = SpawnBuilding(settings.BuildingOptions[index].buildingPrefab.name, SetSpawnLocation(), Quaternion.identity);
        spawnedBuildings.Add(spawnedBuilding);
        buildingHasBeenSpawned = true;
    }

    private void MoveBuildingToCursor()
    {
        float moveSpeed = 1f;
        var currentBuilding = spawnedBuildings[^1];
        
        Vector3 mousePosition = SetSpawnLocation();
        Vector3 buildingMovePostion = new Vector3 (mousePosition.x, 0f, mousePosition.z);

        //Debug.Log(mousePosition.y);
        //mousePosition.y = 0;


        Vector3 buildingCurrentPosition = currentBuilding.transform.position;
        Vector3 offset = new Vector3(-2f, 0f, -2f);


        currentBuilding.transform.position = Vector3.Lerp(buildingCurrentPosition, buildingMovePostion , moveSpeed);
        //currentBuilding.transform.position = buildingMovePostion; + offset
    }
}

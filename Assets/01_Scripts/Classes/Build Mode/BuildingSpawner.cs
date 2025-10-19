using Mono.Cecil;
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
    private Vector3 buildingCurrentPosition;
    private Vector3 boxSize;

    public bool buildingHasBeenSpawned = false;
    public bool buildingHasBeenPlaced = false;
    public bool buildingCanBePlaced = false;
    public bool buildModeActive = false;
    private bool hit;

    private GameObject currentBuilding;

    //public BuildMode buildModeState;
    //public enum BuildMode
    //{
    //    active,
    //    buildingSpawned,
    //    buildingPlaced,
    //    notActive
    //}


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
            buildModeActive = true;
            MoveBuildingToCursor();
            BuildingCollisionChecks();
        }
        else if (buildingHasBeenPlaced)
        {
            PlaceBuilding();
            buildModeActive = false;
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

    //Move Current selected building to follow cursor until placed
    private void MoveBuildingToCursor()
    {
        //grab mouse position
        Vector3 mousePosition = SetSpawnLocation();
        Vector3 buildingMovePostion = new Vector3 (mousePosition.x, 0f, mousePosition.z);
        //grab building current position
        currentBuilding = spawnedBuildings[^1];
        buildingCurrentPosition = currentBuilding.transform.position;
       
        //offset - not currently being used
        //Vector3 offset = new Vector3(-2f, 0f, -2f);

        //move building
        float moveSpeed = 1f;
        currentBuilding.transform.position = Vector3.Lerp(buildingCurrentPosition, buildingMovePostion , moveSpeed);
    }

    //check if building is currently colliding with anything
    private void BuildingCollisionChecks()
    {
        boxSize = currentBuilding.GetComponent<BoxCollider>().size;
        hit = Physics.CheckBox(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize, currentBuilding.transform.rotation, LayerMask.GetMask("Obstacle")); //
        //using Box Collider center to add offset equal to box collider position

        if (hit)
        {
            buildingCanBePlaced = true;
        }
        else
        {
            buildingCanBePlaced = false;
        }
    }

    private void PlaceBuilding()
    { 
        //var currentBuilding = spawnedBuildings[^1];
        currentBuilding.GetComponent<BoxCollider>().enabled = true;

        //reset variables &or spawn another building of same type (so u can place multiple in one go)
    }

    private void OnDrawGizmos()
    {
        if (buildModeActive == true)
        {
            Gizmos.color = hit ? Color.red : Color.yellow;
            Gizmos.DrawCube(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize);
        }
    }
}

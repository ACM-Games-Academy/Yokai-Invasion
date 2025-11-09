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

    public List<GameObject> SpawnedBuildings = new List<GameObject>();

    private Vector3 spawnLocation;
    private Vector3 buildingCurrentPosition;
    private Vector3 boxSize;

    private bool hit;
    private GameObject currentBuilding;

    public Dictionary<string, int> IndexDictionary;

    public bool IsPlaceable() => !hit;

    public BuildMode BuildModeState;
    public enum BuildMode : byte
    {
        inactive,
        active,
        buildingSpawned
    }

    private void Awake()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;

        IndexDictionary = new Dictionary<string, int>();
        var prefabs = settings.BuildingOptions;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (IndexDictionary.ContainsKey(prefabs[i].BuildingPrefab.name)) continue;
            IndexDictionary[prefabs[i].BuildingPrefab.name] = i;
        }
    }

    private void Start()
    {
        //on Start make object pool of buildings
        foreach (var building in settings.BuildingOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(building.BuildingPrefab, settings.PoolSize);
        }
    }

    private void FixedUpdate()
    {
        if (BuildModeState == BuildMode.buildingSpawned)
        {
            MoveBuildingToCursor();
            BuildingCollisionChecks();
        }
    }


    //Get building from object pool
    private GameObject SpawnBuilding(string key, Vector3 position, Quaternion rotation)
    {
        var spawnedBuilding = Overseer.Instance.GetManager<ObjectPooler>().GetPooledObject(key, position, rotation);

        return spawnedBuilding;
    }

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
        // Resource check - how expenesive is it , do u have the rsources

        var spawnedBuilding = SpawnBuilding(settings.BuildingOptions[index].BuildingPrefab.name, SetSpawnLocation(), Quaternion.identity);
        SpawnedBuildings.Add(spawnedBuilding);
        BuildModeState = BuildMode.buildingSpawned;
    }

    private void MoveBuildingToCursor()
    {
        Vector3 mousePosition = SetSpawnLocation();
        Vector3 buildingMovePostion = new Vector3 (mousePosition.x, 0f, mousePosition.z);
        
        currentBuilding = SpawnedBuildings[^1];
        buildingCurrentPosition = currentBuilding.transform.position;

        float moveSpeed = 1f;
        currentBuilding.transform.position = Vector3.Lerp(buildingCurrentPosition, buildingMovePostion , moveSpeed);
    }

    private void BuildingCollisionChecks()
    {
        boxSize = currentBuilding.GetComponent<BoxCollider>().size;
        hit = Physics.CheckBox(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize, currentBuilding.transform.rotation, LayerMask.GetMask("Obstacle")); //mkae custom layermask, buildings, obstacles, player, do not include floor or units
        //using Box Collider center to add offset equal to box collider position
    }

    public void PlaceBuilding()
    { 
        currentBuilding.GetComponent<BoxCollider>().enabled = true;
        BuildModeState = BuildMode.inactive;
        SpawnAtIndex(IndexDictionary[currentBuilding.name]);

        //pull rsource cost here

    }

    private void OnDrawGizmos()
    {
        if (BuildModeState == BuildMode.buildingSpawned)
        {
            Gizmos.color = hit ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize);
        }
    }
}

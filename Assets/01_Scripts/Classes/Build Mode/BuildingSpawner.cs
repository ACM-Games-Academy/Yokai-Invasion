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

    private bool hit;
    private GameObject currentBuilding;

    public Dictionary<string, int> indexDictionary;

    public bool isPlaceable() => !hit;

    public BuildMode buildModeState;
    public enum BuildMode : byte
    {
        inactive,
        active,
        buildingSpawned
    }


    private void Awake()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;  
        
        indexDictionary = new Dictionary<string, int>();
        var prefabs = settings.BuildingOptions;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (indexDictionary.ContainsKey(prefabs[i].buildingPrefab.name)) continue;
            indexDictionary[prefabs[i].buildingPrefab.name] = i;
        }
    }

    private void Start()
    {
        //on Start make object pool of buildings
        foreach (var building in settings.BuildingOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(building.buildingPrefab, settings.PoolSize);
        }
    }

    private void FixedUpdate()
    {
        if (buildModeState == BuildMode.buildingSpawned)
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

        var spawnedBuilding = SpawnBuilding(settings.BuildingOptions[index].buildingPrefab.name, SetSpawnLocation(), Quaternion.identity);
        spawnedBuildings.Add(spawnedBuilding);
        buildModeState = BuildMode.buildingSpawned;
    }

    private void MoveBuildingToCursor()
    {
        Vector3 mousePosition = SetSpawnLocation();
        Vector3 buildingMovePostion = new Vector3 (mousePosition.x, 0f, mousePosition.z);
        
        currentBuilding = spawnedBuildings[^1];
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
        buildModeState = BuildMode.inactive;
        SpawnAtIndex(indexDictionary[currentBuilding.name]);

        //pull rsource cost here

    }

    private void OnDrawGizmos()
    {
        if (buildModeState == BuildMode.buildingSpawned)
        {
            Gizmos.color = hit ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize);
        }
    }
}

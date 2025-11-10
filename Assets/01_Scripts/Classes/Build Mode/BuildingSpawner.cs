using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSpawner : MonoBehaviour
{
    private BuildingSettings settings;

    public List<GameObject> SpawnedBuildings = new List<GameObject>();

    private Vector3 spawnLocation;
    private Vector3 buildingCurrentPosition;
    private Vector3 boxSize;

    private int goldCost;
    private int woodCost;

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
            currentBuilding = SpawnedBuildings[^1];
            if (currentBuilding != null)
            {
                MoveBuildingToCursor();
                BuildingCollisionChecks();
            }
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
            var spawnedBuilding = SpawnBuilding(settings.BuildingOptions[index].BuildingPrefab.name, SetSpawnLocation(), Quaternion.identity);
            SpawnedBuildings.Add(spawnedBuilding);
            BuildModeState = BuildMode.buildingSpawned;

            goldCost = settings.BuildingOptions[index].GoldCost;
            woodCost = settings.BuildingOptions[index].WoodCost;
    }

    private void MoveBuildingToCursor()
    {
        Vector3 mousePosition = SetSpawnLocation();
        Vector3 buildingMovePostion = new Vector3 (mousePosition.x, 0f, mousePosition.z);
        

        buildingCurrentPosition = currentBuilding.transform.position;

        float moveSpeed = 1f;
        currentBuilding.transform.position = Vector3.Lerp(buildingCurrentPosition, buildingMovePostion, moveSpeed);
    }

    private void BuildingCollisionChecks()
    {
        boxSize = currentBuilding.GetComponent<BoxCollider>().size;
        hit = Physics.CheckBox(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize, currentBuilding.transform.rotation, LayerMask.GetMask("Obstacle")); //mkae custom layermask, buildings, obstacles, player, do not include floor or units
        //using Box Collider center to add offset equal to box collider position
    }

    public void PlaceBuilding()
    { 
        if (currentBuilding != null)
        {
            currentBuilding.GetComponent<BoxCollider>().enabled = true;
            BuildModeState = BuildMode.inactive;
            
            PayResources(goldCost,woodCost);

            SpawnAtIndex(IndexDictionary[currentBuilding.name]);
        }
        else { BuildModeState = BuildMode.inactive;}

        //Debug.Log($"Build Mode State from PLACE BUILDING is: '{BuildModeState}'");
    }

    public void CallPlacementPopup() //this cant be in BuildModeInput bc the static TogglePlaceBuilding doesnt like that
    {
        Debug.LogWarning("Cannot place this building here! There is something in the way.");
        StartCoroutine(Overseer.Instance.GetManager<BuildModeInput>().CannotPlaceHerePopup());
    }

    public void ResourceCheck()
    { 
        if (Overseer.Instance.GetManager<ResourceManager>().CurrentGold() >= goldCost && Overseer.Instance.GetManager<ResourceManager>().CurrentWood() >= woodCost)
        {
            PlaceBuilding();
        }   
        else
        {
            Debug.LogWarning("Not enough resources to place this building!");
            StartCoroutine(Overseer.Instance.GetManager<BuildModeInput>().NotEnoughResourcesPopup());
        }
    }

    private void PayResources(int gold, int wood)
    {
        Overseer.Instance.GetManager<ResourceManager>().DecreaseGold(gold);
        Overseer.Instance.GetManager<ResourceManager>().DecreaseWood(wood);
    }

    private void OnDrawGizmos()
    {
        if (BuildModeState == BuildMode.buildingSpawned && currentBuilding != null)
        {
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawWireCube(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize);
        }
    }
}

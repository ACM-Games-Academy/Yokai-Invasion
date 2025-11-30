using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class BuildingSpawner : MonoBehaviour
{
    private BuildingSettings settings;

    public Dictionary<string, int> IndexDictionary;
    public List<GameObject> SpawnedBuildings = new List<GameObject>();

    private Vector3 mousePos;
    private Vector3 buildingCurrentPosition;
    private Vector3 boxSize;

    private GameObject currentBuilding;

    private int goldCost;
    private int woodCost;

    private const int BUILDING_MOVE_SPEED = 1;

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
        foreach (var building in settings.BuildingOptions)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(building.BuildingPrefab, settings.PoolSize);
        }
    }

    public void SpawnByIndex(int index)
    {
        var spawnedBuilding = Overseer.Instance.GetManager<ObjectPooler>().GetPooledObject(
                                                                    settings.BuildingOptions[index].BuildingPrefab.name,
                                                                    GetMousePosition(),
                                                                    Quaternion.identity);
        currentBuilding = spawnedBuilding;
        SpawnedBuildings.Add(currentBuilding);

        BuildModeState = BuildMode.buildingSpawned;

        

        goldCost = settings.BuildingOptions[index].GoldCost;
        woodCost = settings.BuildingOptions[index].WoodCost;
    }

    private Vector3 GetMousePosition()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo))
        {
            mousePos = hitInfo.point;
        }
        return mousePos;
    }

    private void FixedUpdate()
    {
        if (BuildModeState != BuildMode.buildingSpawned) return;
        if (currentBuilding == null) return;

        MoveBuildingToCursor();
    }

    private void MoveBuildingToCursor()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3 buildingMovePostion = new Vector3(mousePosition.x, settings.SpawnHeight, mousePosition.z);

        buildingCurrentPosition = currentBuilding.transform.position;

        currentBuilding.transform.position = Vector3.Lerp(buildingCurrentPosition, buildingMovePostion, BUILDING_MOVE_SPEED);
    }

    /// <summary>
    /// Checks for whether the building is overlapping somethings collision
    /// </summary>
    /// <returns>
    /// true if something is colliding,
    /// false otherwise
    /// </returns>
    public bool BuildingCollisionChecks()
    {
        boxSize = currentBuilding.GetComponent<BoxCollider>().size;
        bool hit = Physics.CheckBox(
                            buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, 
                            boxSize, 
                            currentBuilding.transform.rotation, 
                            LayerMask.GetMask("Obstacle")); //make custom layermask, buildings, obstacles, player, do not include floor or units
        return hit;
        
    }

    public void AttemptToPlaceBuilding()
    {

        var uiCanvas = Overseer.Instance.GetManager<UiSpawner>().uiCanvas;
        var buildModeInput = uiCanvas.GetComponent<BuildModeInput>();

        if (!ResourceCheck())
        {
            Debug.LogWarning("Not enough resources to place this building!");
            StartCoroutine(buildModeInput.TriggerResourcesWarning());
            return;
        }

        if (BuildingCollisionChecks())
        {
            Debug.LogWarning("Cannot place this building here! There is something in the way.");
            StartCoroutine(buildModeInput.TriggerPlacementWarning());
            return;
        }

        if (currentBuilding == null) return;

        PayResources(goldCost, woodCost);
        SetToConstructing();

        if (IsSpaceLeftInPool(currentBuilding))
        {
            SpawnByIndex(IndexDictionary[currentBuilding.name]);
        }

        //Debug.Log($"Build Mode State from ATTEMPT TO PLACE BUILDING is: '{BuildModeState}'");
    }

    private bool ResourceCheck()
    {
        return (Overseer.Instance.GetManager<ResourceManager>().CurrentGold() >= goldCost &&
            Overseer.Instance.GetManager<ResourceManager>().CurrentWood() >= woodCost);
    }

    private void PayResources(int gold, int wood)
    {
        Overseer.Instance.GetManager<ResourceManager>().DecreaseGold(gold);
        Overseer.Instance.GetManager<ResourceManager>().DecreaseWood(wood);
    }

    private void SetToConstructing()
    {
        currentBuilding.GetComponent<Building>().AlterState(Building.BuildingState.constructing);
        BuildModeState = BuildMode.inactive;
    }


    private bool IsSpaceLeftInPool(GameObject prefabToCheck)
    {
        var numberInPool = settings.PoolSize;
        int spawnedTracker = 0;

        foreach (GameObject building in SpawnedBuildings)
        {
            if (building.name == prefabToCheck.name) spawnedTracker++;
        }

        return spawnedTracker < numberInPool;
    }

    public void CancelBuildingPlacement()
    {
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(currentBuilding);
        BuildModeState = BuildMode.inactive;
    }

    private void OnDrawGizmos()
    {
        if (BuildModeState == BuildMode.buildingSpawned && currentBuilding != null)
        {
            Gizmos.color = !BuildingCollisionChecks() ? Color.red : Color.green;
            Gizmos.DrawWireCube(buildingCurrentPosition + currentBuilding.GetComponent<BoxCollider>().center, boxSize);
        }
    }
}

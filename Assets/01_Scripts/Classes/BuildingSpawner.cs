using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSpawner : MonoBehaviour
{
    private BuildingSettings settings;

    private List<GameObject> spawnedTowers;

    private Vector3 spawnLocation;

    private void Awake()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;
    }

    private void Start()
    {
        //on Start make object pool of Towers
        foreach (var tower in settings.TowerPrefabs)
        {
            Overseer.Instance.GetManager<ObjectPooler>().InitializePool(tower, settings.PoolSize);
        }
    }

    private void Update()
    {

    }

    //Get tower from object pool
    private GameObject SpawnTowers(string tower, Vector3 position, Quaternion rotation)
    {
        var spawnedTowers = Overseer.Instance.GetManager<ObjectPooler>().GetPooledObject(tower, position, rotation);

        return spawnedTowers; //what does it return tho?
    }

    private void SpawnTowerOnClick()
    {
        //var newTower = SpawnTowers(keygoesheregofindthat, spawnLocation, Quaternion.identity);
        //spawnedTowers.Add(newTower);
    }

    //set Spawn Location of Tower
    public void SetSpawnLocation() //public so UI can reference
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo))
        {
            spawnLocation = hitInfo.point;
            Debug.Log("Spawn Tower at " + spawnLocation);
        }
    }
}

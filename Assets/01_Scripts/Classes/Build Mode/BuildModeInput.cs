using UnityEngine;
using UnityEngine.InputSystem;

public class BuildModeInput : MonoBehaviour
{
    private GameObject UiCanvas;

    private void Start()
    {
        UiCanvas = GameObject.Find("UI Canvas"); //this isnt efficient so i should change this later
    }
    private void Update()
    {
        ActivateBuildingsList();
    }
    private void ActivateBuildingsList() //MAGIC NUMBER BS HAPPENING HERE - check here if UI is bugging
    {
        //The children under the UI canvas must be in a specific order for this to work properly, so make sure u dont move them around
        if (Overseer.Instance.GetManager<BuildingSpawner>().buildModeState == BuildingSpawner.BuildMode.active) //1st child under UI Canvas
        {
            UiCanvas.transform.GetChild(0).gameObject.SetActive(true); 
        }
        else
        {
            UiCanvas.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //These functions check for inputs from Input Handler
    public static void ToggleBuildingsList(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        switch (Overseer.Instance.GetManager<BuildingSpawner>().buildModeState)
        {
            case BuildingSpawner.BuildMode.active:
                Overseer.Instance.GetManager<BuildingSpawner>().buildModeState = BuildingSpawner.BuildMode.inactive;
                break;
            case BuildingSpawner.BuildMode.inactive:
                Overseer.Instance.GetManager<BuildingSpawner>().buildModeState = BuildingSpawner.BuildMode.active;
                break;
        }
    }

    public static void TogglePlaceBuilding(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        if (Overseer.Instance.GetManager<BuildingSpawner>().buildModeState == BuildingSpawner.BuildMode.buildingSpawned && Overseer.Instance.GetManager<BuildingSpawner>().isPlaceable())
        {
            Overseer.Instance.GetManager<BuildingSpawner>().PlaceBuilding();
        }
    }

}

using UnityEngine;
using UnityEngine.InputSystem;

public class BuildModeInput : MonoBehaviour
{
    private static bool buildMenuIsOpen = false;

    private GameObject UiCanvas;

    private void Start()
    {
        UiCanvas = GameObject.Find("UI Canvas"); //this isnt efficient so i should change this later
    }
    private void Update()
    {
        ActivateBuildingsList();
    }
    private void ActivateBuildingsList()
    {
        
        if (buildMenuIsOpen == true && Overseer.Instance.GetManager<BuildingSpawner>().buildingHasBeenSpawned == false)
        {
            UiCanvas.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            UiCanvas.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //These functions check for inputs from Input Handler
    public static void ToggleBuildingsList(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            buildMenuIsOpen = !buildMenuIsOpen;
        }
    }

    public static void TogglePlaceBuilding(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            if (Overseer.Instance.GetManager<BuildingSpawner>().buildingHasBeenSpawned == true && Overseer.Instance.GetManager<BuildingSpawner>().buildingCanBePlaced == true)
            {
                Debug.Log("has been triggered");
                Overseer.Instance.GetManager<BuildingSpawner>().buildingHasBeenPlaced = true;
                Debug.Log(Overseer.Instance.GetManager<BuildingSpawner>().buildingHasBeenPlaced);
            }
        }
    }

}

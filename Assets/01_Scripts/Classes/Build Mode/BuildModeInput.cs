using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BuildModeInput : MonoBehaviour
{
    [Header("UI Children")]
    //[SerializeField]
    public static GameObject buildMenu;

    private static BuildingSpawner buildingSpawner;
    private static GameObject resourcesWarningPopup;
    private static GameObject placementWarningPopup;

    private float warningWaitTime = 2.5f;

    private const int BUILDMENU_SPAWN_HEIGHT = 140;

    private void Start()
    {
        buildingSpawner = Overseer.Instance.GetManager<BuildingSpawner>();
        buildMenu = GameObject.Find("Build Menu");
        buildMenu.SetActive(false);
    }

    public IEnumerator TriggerResourcesWarning() //this really shouldnt be here but i couldnt be bothered to move it
    {
        resourcesWarningPopup.SetActive(true);
        yield return new WaitForSeconds(warningWaitTime);
        resourcesWarningPopup.SetActive(false);
    }
    public IEnumerator TriggerPlacementWarning() //this too
    {
        placementWarningPopup.SetActive(true);
        yield return new WaitForSeconds(warningWaitTime);
        placementWarningPopup.SetActive(false);
    }


    //The following functions check for inputs from Input Handler ------------------------------

    public static void ToggleBuildMenu(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        switch (buildingSpawner.BuildModeState)
        {
            case BuildingSpawner.BuildMode.active:
                buildMenu.SetActive(!buildMenu.activeSelf);
                buildingSpawner.BuildModeState = BuildingSpawner.BuildMode.inactive;
                break;
            case BuildingSpawner.BuildMode.inactive:
                buildMenu.SetActive(!buildMenu.activeSelf);
                buildingSpawner.BuildModeState = BuildingSpawner.BuildMode.active;
                break;
            case BuildingSpawner.BuildMode.buildingSpawned:
                buildingSpawner.CancelBuildingPlacement();
                buildMenu.SetActive(false);
                break;
        }
    }

    public static void PlaceBuilding(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        buildingSpawner.AttemptToPlaceBuilding();
    }
}

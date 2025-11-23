using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public void ReceiveMovementInput(InputAction.CallbackContext input)
    {
        HeroMovement.SyncMovementInput(input);
    }

    public void ReceiveLMBInput(InputAction.CallbackContext input)
    {
        SelectionInput.HandleSelectionInput(input);
    }

    public void ReceiveRMBInput(InputAction.CallbackContext input)
    {
        if (Overseer.Instance.GetManager<BuildingSpawner>().BuildModeState == BuildingSpawner.BuildMode.inactive)
        {
            CommandManager.HandleMoveInput(input);
            //Debug.Log("CommandManager is receiving RMB input");
        }
        else if (Overseer.Instance.GetManager<BuildingSpawner>().BuildModeState == BuildingSpawner.BuildMode.buildingSpawned)
        {
            BuildModeInput.PlaceBuilding(input);
            //Debug.Log("BuildModeInput is receiving RMB input");
        }
    }

    public void ReceiveModifyInput(InputAction.CallbackContext input)
    {
        SelectionInput.ToggleModify(input);
    }

    public void ReceivePauseInput(InputAction.CallbackContext input) 
    {
        PauseMenu.TogglePause(input);
    }

    public void ReceiveBuildMenuInput(InputAction.CallbackContext input)
    {
        BuildModeInput.ToggleBuildMenu(input);
    }
    public void ReceiveHiringMenuInput(InputAction.CallbackContext input)
    {
        HiringMenuInput.ToggleHiringMenu(input);
    }
    public void ReceiveDebugMenuInput(InputAction.CallbackContext input)
    {
        DebugMenuInput.ToggleDebugMenu(input);
    }

    public void ToggleCameraFollow(InputAction.CallbackContext input)
    {
        CameraMove.ToggleFollowPlayer(input);
    }
}

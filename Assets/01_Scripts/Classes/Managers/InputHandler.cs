using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public void RecieveMovementInput(InputAction.CallbackContext input)
    {
        HeroMovement.SyncMovementInput(input);
    }

    public void RecieveLMBInput(InputAction.CallbackContext input)
    {
        SelectionInput.HandleSelectionInput(input);
    }

    public void RecieveRMBInput(InputAction.CallbackContext input)
    {
        if (Overseer.Instance.GetManager<BuildingSpawner>().buildModeActive == false)
        {
            CommandManager.HandleMoveInput(input);
            Debug.Log("CommandManager is receiving RMB input");
        }
        else if (Overseer.Instance.GetManager<BuildingSpawner>().buildModeActive == true)
        {
            BuildModeInput.TogglePlaceBuilding(input);
            Debug.Log("BuildModeInput is receiving RMB input");
        }
    }

    public void RecieveModifyInput(InputAction.CallbackContext input)
    {
        SelectionInput.ToggleModify(input);
    }

    public void RecievePauseInput(InputAction.CallbackContext input) 
    {
        PauseMenu.TogglePause(input);
    }

    public void ReceiveBuildModeInput(InputAction.CallbackContext input)
    {
        BuildModeInput.ToggleBuildingsList(input);
    }
}

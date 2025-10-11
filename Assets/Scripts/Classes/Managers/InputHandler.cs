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
        CommandManager.HandleMoveInput(input);
    }

    public void RecieveModifyInput(InputAction.CallbackContext input)
    {
        SelectionInput.ToggleModify(input);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class CommandManager : MonoBehaviour
{
    public static void HandleMoveInput(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo))
        {
            var destination = hitInfo.point;

            foreach (var unit in Overseer.Instance.GetManager<SelectionManager>().SelectedUnits)
            {
                unit.SetDestination(destination);
            }
        }
    }
}

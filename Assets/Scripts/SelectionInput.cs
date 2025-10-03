using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionInput : MonoBehaviour
{
    public void HandleSelectionInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo) &&
            hitInfo.collider.TryGetComponent<SelectableUnit>(out SelectableUnit Unit))
        {
            Debug.Log($"Clicked on {Unit.name}");
            SelectionManager.Instance.Select(Unit);
        }
        else
        {
            Debug.Log("Clicked on empty space, clearing selection");
            SelectionManager.Instance.ClearSelection();
        }
    }
}

using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionInput : MonoBehaviour
{

    private bool isModifying = false;

    public void HandleSelectionInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo) &&
            hitInfo.collider.TryGetComponent<SelectableUnit>(out SelectableUnit Unit))
        {
            if (isModifying && SelectionManager.Instance.IsSelected(Unit))
            {
                Debug.Log($"Deselecting {Unit.name}");
                SelectionManager.Instance.Deselect(Unit);
                return;
            }
            else if (isModifying && !SelectionManager.Instance.IsSelected(Unit))
            {
                Debug.Log($"Adding {Unit.name} to selection");
                SelectionManager.Instance.Select(Unit);
                return;
            }
            else
            {
                Debug.Log($"Clicked on {Unit.name}");
                SelectionManager.Instance.ClearSelection();
                SelectionManager.Instance.Select(Unit);
            }
        }
        else
        {
            Debug.Log("Clicked on empty space, clearing selection");
            SelectionManager.Instance.ClearSelection();
        }
    }

    public void ToggleModify(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Modify key pressed");
            isModifying = true;
        }
        else if (context.canceled)
        {
            Debug.Log("Modify key released");
            isModifying = false;
        }
    }
}

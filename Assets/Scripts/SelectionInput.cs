using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionInput : MonoBehaviour
{
    private bool isModifying = false;
    [SerializeField] private RectTransform selectionBox;
    private Vector2 initialMousePosition;

    private float dragDelay = 0.1f;
    private float dragStartTime;

    private enum ClickState
    {
        None,
        Click_Started,
        Click_Holding,
        Click_Ending_From_Hold,
        Click_Ending_From_Click
    }
    private ClickState currentClickState = ClickState.None;

    private void Update()
    {
        if ((currentClickState == ClickState.Click_Holding || currentClickState == ClickState.Click_Started)
             && dragStartTime + dragDelay <= Time.time)
        {
            currentClickState = ClickState.Click_Holding;
        }

        switch (currentClickState)
        {
            case ClickState.None:
                break;

            case ClickState.Click_Holding:
                selectionBox.gameObject.SetActive(true);
                changeSelectionArea();
                break;

            case ClickState.Click_Ending_From_Hold:
                selectionBox.gameObject.SetActive(false);
                currentClickState = ClickState.None;
                break;

            case ClickState.Click_Ending_From_Click:
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo) &&
                hitInfo.collider.TryGetComponent<SelectableUnit>(out SelectableUnit Unit))
                {
                    if (isModifying && SelectionManager.Instance.IsSelected(Unit))
                    {
                        Debug.Log($"Deselecting {Unit.name}");
                        SelectionManager.Instance.Deselect(Unit);
                    }
                    else if (isModifying && !SelectionManager.Instance.IsSelected(Unit))
                    {
                        Debug.Log($"Adding {Unit.name} to selection");
                        SelectionManager.Instance.Select(Unit);
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
                currentClickState = ClickState.None;
                break;
        }
    }

    public void HandleSelectionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            initialMousePosition = Mouse.current.position.ReadValue();
            selectionBox.anchoredPosition = initialMousePosition;
            selectionBox.sizeDelta = Vector2.zero;
            dragStartTime = Time.time;
            currentClickState = ClickState.Click_Started;
        }
        else if (context.canceled)
        {
            if (currentClickState == ClickState.Click_Holding)
            {
                currentClickState = ClickState.Click_Ending_From_Hold;
            }
            else if (currentClickState == ClickState.Click_Started)
            {
                currentClickState = ClickState.Click_Ending_From_Click;
            }
            else
            {
                currentClickState = ClickState.None;
            }
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

    private void changeSelectionArea()
    {
        float width = Mouse.current.position.x.ReadValue() - initialMousePosition.x;
        float height = Mouse.current.position.y.ReadValue() - initialMousePosition.y;

        selectionBox.anchoredPosition = initialMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        foreach (var Unit in SelectionManager.Instance.AvailableUnits)
        {
            if (bounds.Contains(Camera.main.WorldToScreenPoint(Unit.transform.position)))
            {
                if (!SelectionManager.Instance.IsSelected(Unit))
                {
                    Debug.Log($"Selecting {Unit.name} via box");
                    SelectionManager.Instance.Select(Unit);
                }
            }
            else
            {
                if (SelectionManager.Instance.IsSelected(Unit))
                {
                    Debug.Log($"Deselecting {Unit.name} via box");
                    SelectionManager.Instance.Deselect(Unit);
                }
            }
        }
    }
}

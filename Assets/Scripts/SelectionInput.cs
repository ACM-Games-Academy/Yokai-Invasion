using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionInput : MonoBehaviour
{
    private bool isModifying = false;
    [SerializeField] private RectTransform selectionBox;
    private Vector2 initialMousePosition;

    private float dragDelay = 0.25f;
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
        if (currentClickState == ClickState.Click_Started && dragStartTime + dragDelay <= Time.time)
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
            isModifying = true;
        }
        else if (context.canceled)
        {
            isModifying = false;
        }
    }

    private void changeSelectionArea()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float width = mousePos.x - initialMousePosition.x;
        float height = mousePos.y - initialMousePosition.y;

        selectionBox.anchoredPosition = initialMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Rect rect = new Rect(
            Mathf.Min(initialMousePosition.x, mousePos.x),
            Mathf.Min(initialMousePosition.y, mousePos.y),
            Mathf.Abs(width),
            Mathf.Abs(height)
        );

        if (!isModifying)
        {
            SelectionManager.Instance.ClearSelection();
        }

        foreach (var Unit in SelectionManager.Instance.AvailableUnits)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Unit.transform.position);

            if (rect.Contains(screenPos))
            {
                if (!SelectionManager.Instance.IsSelected(Unit))
                {
                    Debug.Log($"Selecting {Unit.name} via box");
                    SelectionManager.Instance.Select(Unit);
                }
            }
            else if (isModifying && SelectionManager.Instance.IsSelected(Unit))
            {
                Debug.Log($"Deselecting {Unit.name} via box");
                SelectionManager.Instance.Deselect(Unit);
            }
        }
    }

}

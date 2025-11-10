using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionInput : MonoBehaviour
{
    private static bool isModifying = false;
    private static Vector2 initialMousePosition;

    private static float dragDelay = 0.25f;
    private static float dragStartTime;

    private static RectTransform selectionBox;

    private void Start()
    {
        GameObject canvasInstance = Instantiate(Overseer.Instance.Settings.SelectionCanvas, transform);

        selectionBox = canvasInstance.transform.Find("SelectionBox").GetComponent<RectTransform>();
    }

    private enum ClickState
    {
        None,
        Click_Started,
        Click_Holding,
        Click_Ending_From_Hold,
        Click_Ending_From_Click
    }
    private static ClickState currentClickState = ClickState.None;

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
                    if (isModifying && Overseer.Instance.GetManager<SelectionManager>().IsSelected(Unit))
                    {
                        //Debug.Log($"Deselecting {Unit.name}");
                        Overseer.Instance.GetManager<SelectionManager>().Deselect(Unit);
                    }
                    else if (isModifying && !Overseer.Instance.GetManager<SelectionManager>().IsSelected(Unit))
                    {
                        //Debug.Log($"Adding {Unit.name} to selection");
                        Overseer.Instance.GetManager<SelectionManager>().Select(Unit);
                    }
                    else
                    {
                        //Debug.Log($"Clicked on {Unit.name}");
                        Overseer.Instance.GetManager<SelectionManager>().ClearSelection();
                        Overseer.Instance.GetManager<SelectionManager>().Select(Unit);
                    }
                }
                else
                {
                    //Debug.Log("Clicked on empty space, clearing selection");
                    Overseer.Instance.GetManager<SelectionManager>().ClearSelection();
                }
                currentClickState = ClickState.None;
                break;
        }
    }

    public static void HandleSelectionInput(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            initialMousePosition = Mouse.current.position.ReadValue();
            selectionBox.anchoredPosition = initialMousePosition;
            selectionBox.sizeDelta = Vector2.zero;
            dragStartTime = Time.time;
            currentClickState = ClickState.Click_Started;
        }
        else if (input.canceled)
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

    public static void ToggleModify(InputAction.CallbackContext context)
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
            Overseer.Instance.GetManager<SelectionManager>().ClearSelection();
        }

        foreach (var Unit in Overseer.Instance.GetManager<SelectionManager>().AvailableUnits)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Unit.transform.position);

            if (rect.Contains(screenPos))
            {
                if (!Overseer.Instance.GetManager<SelectionManager>().IsSelected(Unit))
                {
                    //Debug.Log($"Selecting {Unit.name} via box");
                    Overseer.Instance.GetManager<SelectionManager>().Select(Unit);
                }
            }
            else if (isModifying && Overseer.Instance.GetManager<SelectionManager>().IsSelected(Unit))
            {
                //Debug.Log($"Deselecting {Unit.name} via box");
                Overseer.Instance.GetManager<SelectionManager>().Deselect(Unit);
            }
        }
    }
}

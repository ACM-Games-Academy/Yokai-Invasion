using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public HashSet<SelectableUnit> SelectedUnits = new HashSet<SelectableUnit>(); // HashSet to avoid duplicates
    public List<SelectableUnit> AvailableUnits = new List<SelectableUnit>();

    private CommandManager commandManager;
    private SelectionInput selectionInput;

    private GameObject selectionGameObject;

    private void Awake()
    {
        var commandManagerType = typeof(CommandManager);

        GameObject commandManagerObject = new GameObject(commandManagerType.Name);
        commandManagerObject.transform.SetParent(transform);

        commandManager = commandManagerObject.AddComponent<CommandManager>();

        var selectionInputType = typeof(SelectionInput);

        GameObject selectionInputObject = new GameObject(selectionInputType.Name);
        selectionInputObject.transform.SetParent(transform);

        selectionInput = selectionInputObject.AddComponent<SelectionInput>();
    }

    private void Start()
    {
        selectionInput.SetSelectionBoxPrefab(selectionGameObject);
    }

    public void Select(SelectableUnit Unit)
    {
        Unit.OnSelect();
        SelectedUnits.Add(Unit);
        Debug.Log($"Selected {Unit.name}. Total selected units: {SelectedUnits.Count}");
    }

    public void Deselect(SelectableUnit Unit)
    {
        Unit.OnDeselect();
        SelectedUnits.Remove(Unit);
    }

    public bool IsSelected(SelectableUnit Unit)
    {
        return SelectedUnits.Contains(Unit);
    }

    public void ClearSelection()
    {
        foreach (var Unit in SelectedUnits)
        {
            Unit.OnDeselect();
        }
        SelectedUnits.Clear();
    }

    public void SetSelectionCanvas(GameObject canvas)
    {
        selectionGameObject = canvas;
    }
}

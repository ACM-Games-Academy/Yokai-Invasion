using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public HashSet<SelectableUnit> SelectedUnits = new HashSet<SelectableUnit>(); // HashSet to avoid duplicates
    public List<SelectableUnit> AvailableUnits = new List<SelectableUnit>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
}

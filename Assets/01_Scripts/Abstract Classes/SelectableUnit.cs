using Unity.VisualScripting;
using UnityEngine;

public abstract class SelectableUnit : Unit
{
    private void Start()
    {
        Overseer.Instance.GetManager<SelectionManager>().AvailableUnits.Add(this);
    }

    private void OnDisable()
    {
        Overseer.Instance.GetManager<SelectionManager>().AvailableUnits.Remove(this);
        Overseer.Instance.GetManager<SelectionManager>().Deselect(this);
    }

    public void OnSelect()
    {
        // Add visual indication of selection
    }

    public void OnDeselect()
    {
        // Remove visual indication of selection
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    private void Start()
    {
        SelectionManager.Instance.AvailableUnits.Add(this);
    }

    private void OnDisable()
    {
        SelectionManager.Instance.AvailableUnits.Remove(this);
        SelectionManager.Instance.Deselect(this);
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

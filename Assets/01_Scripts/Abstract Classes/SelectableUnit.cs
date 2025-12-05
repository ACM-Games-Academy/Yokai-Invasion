using Unity.VisualScripting;
using UnityEngine;

public abstract class SelectableUnit : Unit
{
    [SerializeField]
    private GameObject selectionIndicator;

    private AudioSettings audioSettings;

    protected void Start()
    {
        Overseer.Instance.GetManager<SelectionManager>().AvailableUnits.Add(this);

        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }

    private void OnDisable()
    {
        Overseer.Instance.GetManager<SelectionManager>().AvailableUnits.Remove(this);
        Overseer.Instance.GetManager<SelectionManager>().Deselect(this);
    }

    public void OnSelect()
    {
        // Add visual indication of selection
        selectionIndicator.SetActive(true);

        //  [25]  Play_Select_Unit - Plays pencil scratching noise
        audioSettings.Events[25].Post(gameObject);
    }

    public void OnDeselect()
    {
        // Remove visual indication of selection
        selectionIndicator.SetActive(false);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public abstract class SelectableUnit : MonoBehaviour
{
    protected Vector3[] currentPath;
    protected int currentWaypointIndex;

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

    public void SetDestination(Vector3 destination)
    {
        currentPath = AStar.AStarPath(transform.position, destination);
        currentWaypointIndex = 0;

        if (currentPath == null || currentPath.Length == 0)
        {
            Debug.LogWarning("Farmer: No valid path found!");
            return;
        }

        Debug.Log($"New path with {currentPath.Length} waypoints:");
        for (int i = 0; i < currentPath.Length; i++)
        {
            Debug.Log($"Waypoint {i}: {currentPath[i]}");
        }
    }

}

using Unity.VisualScripting;
using UnityEngine;

public abstract class SelectableUnit : MonoBehaviour
{
    protected Vector3[] currentPath;
    protected int currentWaypointIndex;

    [SerializeField]
    protected UnitSettings settings;

    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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
    }

    private void FixedUpdate()
    {
        var pathDir = Vector3.zero;
        if (currentPath != null && currentWaypointIndex < currentPath.Length)
        {
            Vector3 waypoint = currentPath[currentWaypointIndex];
            pathDir = (waypoint - transform.position).normalized;

            // Advance waypoint if close
            if (Vector3.Distance(transform.position, waypoint) < settings.WaypointTolerance)
                currentWaypointIndex++;

        }

        rb.MovePosition(
            rb.position
            + pathDir.normalized
            * settings.MoveSpeed
            * Time.fixedDeltaTime);
    }
}

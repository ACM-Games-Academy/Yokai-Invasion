using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected UnitSettings settings;

    protected Vector3[] currentPath;
    protected int currentWaypointIndex;

    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetDestination(Vector3 destination)
    {
        currentPath = AStar.Path(transform.position, destination);
        currentWaypointIndex = 0;
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

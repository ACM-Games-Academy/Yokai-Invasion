using UnityEngine;

public class Farmer : SelectableUnit
{
    [SerializeField]
    private UnitSettings settings;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

            Debug.Log($"Current pathDir: {pathDir}");
        }

        rb.MovePosition(
            rb.position
            + pathDir.normalized
            * settings.MoveSpeed
            * Time.fixedDeltaTime);
    }
}

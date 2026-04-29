using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected UnitSettings settings;

    protected Vector3[] currentPath;
    protected int currentWaypointIndex;

    protected Rigidbody rb;
    protected CapsuleCollider unitCollider;

    protected bool isWalking;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        unitCollider = GetComponent<CapsuleCollider>();
    }
    private void OnEnable() //this is so that they dont stack on each other at the spawn point
    {
        var temporaryLocation = new Vector3 (transform.position.x + Random.Range(-7,7), 0, transform.position.z + Random.Range(1,7));
        SetDestination(temporaryLocation);
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

        if (pathDir != Vector3.zero) { isWalking = true; }
        else { isWalking = false; }

        rb.MovePosition(
            rb.position
            + pathDir.normalized
            * settings.MoveSpeed
            * Time.fixedDeltaTime);
    }
}

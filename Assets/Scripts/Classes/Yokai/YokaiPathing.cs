using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;

public class YokaiPathing : MonoBehaviour
{
    private Rigidbody rb;
    private Boids boidScript;
    private AStar aStarScript;

    [SerializeField] private Transform templeLocation;
    private Transform heroTransform;

    private float floorHeight = 0.66f;

    private Vector3[] currentPath;
    private int currentWaypointIndex;

    [SerializeField]
    private YokaiSettings settings;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        heroTransform = GameObject.FindGameObjectWithTag("Hero").transform;
    }

    private void FixedUpdate()
    {
        YokaiState.YokaiStates currentYokaiState = GetComponent<YokaiState>().GetCurrentState();

        Vector3 pathDir = Vector3.zero;

        if (currentPath != null && currentWaypointIndex < currentPath.Length)
        {
            Vector3 waypoint = currentPath[currentWaypointIndex];
            pathDir = (waypoint - transform.position).normalized;

            // Advance waypoint if close
            if (Vector3.Distance(transform.position, waypoint) < settings.WaypointTolerance)
                currentWaypointIndex++;
        }

        // Boid steering vector
        Vector3 boidDir = boidScript.BoidsPath(settings.DetectionRadius, transform.position);

        switch (currentYokaiState)
        {
            case YokaiState.YokaiStates.Idle:
                if (currentPath == null)
                {
                    currentPath = aStarScript.AStarPath(transform.position, templeLocation.position);
                    currentWaypointIndex = 0;
                }

                Move((boidDir * settings.AllyReliance) + (pathDir * (1 - settings.AllyReliance)));
                break;

            case YokaiState.YokaiStates.Pursuing:
                if (currentPath == null || ReachedEnd())
                {
                    currentPath = aStarScript.AStarPath(transform.position, heroTransform.position);
                    currentWaypointIndex = 0;
                }
                Move((boidDir * settings.AllyReliance) + (pathDir * (1 - settings.AllyReliance)));
                break;

            case YokaiState.YokaiStates.Attacking:
                // Stop movement when attacking?
                break;

            case YokaiState.YokaiStates.Fleeing:
                if (currentPath == null || ReachedEnd())
                {
                    Vector3 fleeTarget = transform.position + (transform.position - heroTransform.position).normalized * 10f;
                    currentPath = aStarScript.AStarPath(transform.position, fleeTarget);
                    currentWaypointIndex = 0;
                }
                Move((boidDir * settings.AllyReliance) + (pathDir * (1 - settings.AllyReliance)));
                break;

            case YokaiState.YokaiStates.Dead:
                // Stop all movement when dead
                break;

            default:
                break;
        }
        if (transform.position.y >= floorHeight)
        {
            rb.MovePosition(new Vector3(rb.position.x, floorHeight, rb.position.z));
        }
    }

    private void Move(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        rb.MovePosition(
            rb.position 
            + dir.normalized 
            * settings.MoveSpeed
            * Time.fixedDeltaTime);
    }

    private bool ReachedEnd()
    {
        return currentPath != null && currentWaypointIndex >= currentPath.Length;
    }

    // --- Setup methods ---
    public void SetTempleLocation(Transform position) => templeLocation = position;
    public void SetBoidScript(Boids boids) => boidScript = boids;
    public void SetAStarScript(AStar aStar) => aStarScript = aStar;
}

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

    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waypointTolerance = 0.3f;

    private float floorHeight = 0.66f;

    private float alignmentWeight;
    private float cohesionWeight;
    private float separationWeight;

    [SerializeField]
    private float reliance = 0.5f; // 0 = only boids, 1 = only A*

    private Vector3[] currentPath;
    private int currentWaypointIndex;

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
            if (Vector3.Distance(transform.position, waypoint) < waypointTolerance)
                currentWaypointIndex++;
        }

        // Boid steering vector
        Vector3 boidDir = boidScript.BoidsPath(alignmentWeight, cohesionWeight, separationWeight, detectionRadius);

        switch (currentYokaiState)
        {
            case YokaiState.YokaiStates.Idle:
                if (currentPath == null)
                {
                    currentPath = aStarScript.AStarPath(transform.position, templeLocation.position);
                    currentWaypointIndex = 0;
                }
                Move(boidDir + pathDir);
                break;
            case YokaiState.YokaiStates.Pursuing:
                if (currentPath == null || ReachedEnd())
                {
                    currentPath = aStarScript.AStarPath(transform.position, heroTransform.position);
                    currentWaypointIndex = 0;
                }
                Move(boidDir + pathDir);
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
                Move(boidDir + pathDir);
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
            * speed 
            * Time.fixedDeltaTime);
    }

    private bool ReachedEnd()
    {
        return currentPath != null && currentWaypointIndex >= currentPath.Length;
    }

    // --- Setup methods ---
    public void SetTempleLocation(Transform position) => templeLocation = position;
    public void SetBoidWeights(Vector3 weights) => (alignmentWeight, cohesionWeight, separationWeight) = (weights.x, weights.y, weights.z);
    public void SetBoidScript(Boids boids) => boidScript = boids;
    public void SetAStarScript(AStar aStar) => aStarScript = aStar;
}

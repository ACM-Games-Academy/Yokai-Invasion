using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;

public class YokaiPathing : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 templeLocation = new Vector3(7, 3, -7);
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

        Vector3 pathDir = InitializeAStar();
        Vector3 boidDir = InitializeBoids();

        // State machine for pathing
        switch (currentYokaiState)
        {
            case YokaiState.YokaiStates.Idle:
                Move(CalaculatePath_Idle(boidDir, pathDir));
                break;

            case YokaiState.YokaiStates.Pursuing:
                Move(CalculatePath_Pursuing(boidDir, pathDir));
                break;

            case YokaiState.YokaiStates.Attacking:
                // Stop movement when attacking?
                break;

            case YokaiState.YokaiStates.Fleeing:
                Move(CalculatePath_Fleeing(boidDir, pathDir));
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



    private Vector3 CalaculatePath_Idle(Vector3 boidDir, Vector3 pathDir)
    {
        if (currentPath == null)
        {
            currentPath = AStar.AStarPath(transform.position, templeLocation);
            currentWaypointIndex = 0;
        }

        return ((boidDir * settings.AllyReliance) + (pathDir * (1 - settings.AllyReliance)));
    }

    private Vector3 CalculatePath_Pursuing(Vector3 boidDir, Vector3 pathDir)
    {
        if (currentPath == null || ReachedEnd())
        {
            currentPath = AStar.AStarPath(transform.position, heroTransform.position);
            currentWaypointIndex = 0;
        }
        return (boidDir * settings.AllyReliance) + (pathDir * (1 - settings.AllyReliance));
    }

    private Vector3 CalculatePath_Fleeing(Vector3 boidDir, Vector3 pathDir)
    {
        if (currentPath == null || ReachedEnd())
        {
            Vector3 fleeTarget = transform.position + (transform.position - heroTransform.position).normalized * 10f;
            currentPath = AStar.AStarPath(transform.position, fleeTarget);
            currentWaypointIndex = 0;
        }
        return (boidDir * settings.AllyReliance) + (pathDir * (1 - settings.AllyReliance));
    }

    private Vector3 InitializeBoids()
    {
        var nearbyColliders = Boids.GetNearby(transform.position, settings.DetectionRadius, ~LayerMask.GetMask("Floor"));
        var nearbyPositions = new List<Vector3>();
        var nearbyVelocities = new List<Vector3>();
        var nearbyObstacles = new List<Vector3>();

        foreach (var collider in nearbyColliders)
        {
            if (collider.CompareTag("Yokai"))
            {
                nearbyPositions.Add(collider.transform.position);
                nearbyVelocities.Add(collider.attachedRigidbody != null ? collider.attachedRigidbody.linearVelocity : Vector3.zero);
            }
            else
            {
                nearbyObstacles.Add(collider.transform.position);
            }
        }

        return Boids.BoidsPath(
            transform.position,
            rb.linearVelocity,
            nearbyPositions,
            nearbyVelocities,
            nearbyObstacles
        );
    }

    private Vector3 InitializeAStar()
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
        return pathDir;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class YokaiPathing : MonoBehaviour
{
    private Rigidbody rb;
    private Yokai yokai;

    private Vector3 templeLocation = new Vector3(7, 1, 0);
    private Transform heroTransform;

    private float floorHeight = 0.66f;

    private Vector3[] currentPath;
    private int currentWaypointIndex;

    [SerializeField]
    private YokaiSettings settings;

    // --- Logical velocity tracking ---
    private Vector3 lastPosition;
    [HideInInspector] public Vector3 logicalVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yokai = GetComponent<Yokai>();
        heroTransform = GameObject.FindGameObjectWithTag("Hero").transform;
        currentWaypointIndex = 0;

        lastPosition = transform.position;
        logicalVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Update our logical velocity first
        logicalVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;

        Yokai.States currentYokaiState = yokai.state;

        Vector3 pathDir = InitializeAStar();
        Vector3 boidDir = InitializeBoids();

        Debug.DrawRay(transform.position, boidDir * 5f, Color.green);
        Debug.DrawRay(transform.position, pathDir * 5f, Color.blue);

        // --- State machine for pathing ---
        switch (currentYokaiState)
        {
            case Yokai.States.Idle:
                Move(CalculatePath_Idle(boidDir, pathDir));
                break;

            case Yokai.States.Pursuing:
                Move(CalculatePath_Pursuing(boidDir, pathDir));
                break;

            case Yokai.States.Attacking:
                // Stop movement while attacking
                break;

            case Yokai.States.Fleeing:
                Move(CalculatePath_Fleeing(boidDir, pathDir));
                break;

            case Yokai.States.Dead:
                // Stop all movement when dead
                break;
        }

        // Clamp to floor
        if (transform.position.y >= floorHeight)
        {
            rb.MovePosition(new Vector3(rb.position.x, floorHeight, rb.position.z));
        }
    }

    // --- Movement ---
    private void Move(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        rb.MovePosition(
            rb.position +
            dir.normalized *
            settings.MoveSpeed *
            Time.fixedDeltaTime);
    }

    private bool ReachedEnd()
    {
        return currentPath != null && currentWaypointIndex >= currentPath.Length;
    }

    // --- State path calculations ---
    private Vector3 CalculatePath_Idle(Vector3 boidDir, Vector3 pathDir)
    {
        if (currentPath == null || ReachedEnd())
        {
            currentPath = AStar.Path(transform.position, templeLocation);
            currentWaypointIndex = 0;
        }

        if (currentPath != null && currentWaypointIndex < currentPath.Length)
        {
            Vector3 waypoint = currentPath[currentWaypointIndex];
            pathDir = (waypoint - transform.position).normalized;

            if (Vector3.Distance(transform.position, waypoint) < settings.WaypointTolerance)
                currentWaypointIndex++;
        }

        return (boidDir * settings.AllyReliance) + (pathDir * (1f - settings.AllyReliance));
    }

    private Vector3 CalculatePath_Pursuing(Vector3 boidDir, Vector3 pathDir)
    {
        if (currentPath == null || ReachedEnd())
        {
            currentPath = AStar.Path(transform.position, heroTransform.position);
            currentWaypointIndex = 0;
        }

        if (currentPath != null && currentWaypointIndex < currentPath.Length)
        {
            Vector3 waypoint = currentPath[currentWaypointIndex];
            pathDir = (waypoint - transform.position).normalized;

            if (Vector3.Distance(transform.position, waypoint) < settings.WaypointTolerance)
                currentWaypointIndex++;
        }

        return (boidDir * settings.AllyReliance) + (pathDir * (1f - settings.AllyReliance));
    }

    private Vector3 CalculatePath_Fleeing(Vector3 boidDir, Vector3 pathDir)
    {
        if (currentPath == null)
        {
            Vector3 fleeTarget = transform.position + (transform.position - heroTransform.position).normalized * 50f;
            currentPath = AStar.Path(transform.position, fleeTarget);
            currentWaypointIndex = 0;
        }
        else if (ReachedEnd())
        {
            yokai.SetState(Yokai.States.Idle);
        }

        if (currentPath != null && currentWaypointIndex < currentPath.Length)
        {
            Vector3 waypoint = currentPath[currentWaypointIndex];
            pathDir = (waypoint - transform.position).normalized;

            if (Vector3.Distance(transform.position, waypoint) < settings.WaypointTolerance)
                currentWaypointIndex++;
        }

        return (boidDir * settings.AllyReliance) + (pathDir * (1f - settings.AllyReliance));
    }

    // --- Boid integration ---
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

                var otherPathing = collider.GetComponent<YokaiPathing>();
                if (otherPathing != null)
                    nearbyVelocities.Add(otherPathing.logicalVelocity);
                else
                    nearbyVelocities.Add(Vector3.zero);
            }
            else
            {
                nearbyObstacles.Add(collider.transform.position);
            }
        }

        return Boids.Path(
            transform.position,
            logicalVelocity,
            nearbyPositions,
            nearbyVelocities,
            nearbyObstacles
        );
    }

    private Vector3 InitializeAStar()
    {
        // Make sure we always have a valid path
        if (currentPath == null || currentWaypointIndex >= currentPath.Length)
        {
            Vector3 target = yokai.state switch
            {
                Yokai.States.Pursuing => heroTransform.position,
                Yokai.States.Fleeing => transform.position + (transform.position - heroTransform.position).normalized * 50f,
                _ => templeLocation
            };

            currentPath = AStar.Path(transform.position, target);
            currentWaypointIndex = 0;
        }

        if (currentPath == null || currentPath.Length == 0)
            return Vector3.zero;

        // Compute direction toward next waypoint
        Vector3 waypoint = currentPath[currentWaypointIndex];
        Vector3 dir = (waypoint - transform.position).normalized;

        // Advance waypoint if close
        if (Vector3.Distance(transform.position, waypoint) < settings.WaypointTolerance)
            currentWaypointIndex++;

        return dir;
    }

}

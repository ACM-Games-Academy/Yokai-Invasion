using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;

public class YokaiPathing : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private Transform templeLocation;

    private Transform heroTransform;

    [SerializeField]
    private float detectionRadius = 5f;

    [SerializeField]
    private float speed = 5f;

    private float floorHeight = 0.66f;

    private float alignmentWeight;
    private float cohesionWeight;
    private float separationWeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        heroTransform = GameObject.FindGameObjectWithTag("Hero").transform;
    }

    private void FixedUpdate()
    {
        YokaiState.YokaiStates currentYokaiState = GetComponent<YokaiState>().GetCurrentState();

        switch (currentYokaiState)
        {
            case YokaiState.YokaiStates.Idle:
                rb.MovePosition(rb.position + (BoidsPath() + (templeLocation.position - transform.position).normalized).normalized * speed * Time.fixedDeltaTime);
                break;
            case YokaiState.YokaiStates.Pursuing:
                // Implement pursuing behavior here
                break;
            case YokaiState.YokaiStates.Attacking:
                // Stop movement when attacking
                break;
            case YokaiState.YokaiStates.Fleeing:
                rb.MovePosition(rb.position + (BoidsPath() + (heroTransform.position + transform.position).normalized).normalized * speed * Time.fixedDeltaTime);
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

    private Vector3 BoidsPath()
    {
        Collider[] nearbyObjects = GetNearby(detectionRadius);

        if (nearbyObjects.Length > 0)
        {
            Vector3 alignment = Vector3.zero;
            Vector3 cohesion = Vector3.zero;
            Vector3 separation = Vector3.zero;

            int nearbyYokaiCount = 0;

            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag("Yokai") == true)
                {
                    nearbyYokaiCount++;

                    // Alignment: Match velocity with nearby yokai
                    alignment += obj.attachedRigidbody.linearVelocity;

                    // Cohesion: Move towards the average position of nearby yokai
                    cohesion += obj.transform.position;
                }   

                // Separation: Avoid crowding nearby objects
                Vector3 toObj = transform.position - obj.transform.position;
                if (toObj.magnitude > 0)
                {
                    separation += toObj.normalized / toObj.magnitude;
                }
            }

            alignment /= nearbyYokaiCount;
            cohesion = (cohesion / nearbyYokaiCount - transform.position).normalized;
            separation /= nearbyObjects.Length;

            alignment *= alignmentWeight;
            cohesion *= cohesionWeight;
            separation *= separationWeight;

            // Combine the three behaviors with weights
            Vector3 boidDirection = alignment + cohesion + separation;
            return boidDirection.normalized;
        }

        return Vector3.zero;
    }

    public void SetTempleLocation(Transform position)
    {
        templeLocation = position;
    }

    public void SetBoidWeights(Vector3 weights)
    {
        alignmentWeight = weights.x;
        cohesionWeight = weights.y;
        separationWeight = weights.z;
    }

    public Collider[] GetNearby(float detectionRadius)
    {
        int mask = ~LayerMask.GetMask("Floor"); // all layers EXCEPT floor
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, mask);

        List<Collider> nearby = new List<Collider>();

        foreach (Collider hit in hits)
        {
            // Skip self
            if (hit.transform == transform) continue;

            nearby.Add(hit);
        }
        return nearby.ToArray();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    [SerializeField]
    private float alignmentWeight = 1f;
    [SerializeField]
    private float cohesionWeight = 1f;
    [SerializeField]
    private float separationWeight = 1f;

    public Vector3 BoidsPath(float detectionRadius, Vector3 yokaiPosition)
    {
        Collider[] nearbyObjects = GetNearby(detectionRadius, yokaiPosition);

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

    public Collider[] GetNearby(float detectionRadius, Vector3 yokaiPosition)
    {
        int mask = ~LayerMask.GetMask("Floor"); // all layers EXCEPT floor

        Collider[] hits = Physics.OverlapSphere(yokaiPosition, detectionRadius, mask);

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

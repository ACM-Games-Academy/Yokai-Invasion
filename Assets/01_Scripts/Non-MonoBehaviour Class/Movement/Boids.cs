using System.Collections.Generic;
using UnityEngine;

public static class Boids
{
    private static float alignmentWeight = 1f;
    private static float cohesionWeight = 1f;
    private static float separationWeight = 1f;

    /// <summary>
    /// Calculates the boid steering vector for a single yokai.
    /// </summary>
    /// <param name="yokaiPosition">Position of the yokai using this behavior.</param>
    /// <param name="yokaiVelocity">Current velocity of the yokai.</param>
    /// <param name="nearbyYokaiPositions">Positions of nearby yokai.</param>
    /// <param name="nearbyYokaiVelocities">Velocities of nearby yokai.</param>
    /// <param name="nearbyObstacles">Positions of nearby obstacles for separation.</param>
    /// <returns>Normalized boid direction vector.</returns>
    public static Vector3 BoidsPath(
        Vector3 yokaiPosition,
        Vector3 yokaiVelocity,
        List<Vector3> nearbyYokaiPositions,
        List<Vector3> nearbyYokaiVelocities,
        List<Vector3> nearbyObstacles
    )
    {
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;

        int nearbyCount = nearbyYokaiPositions.Count;

        if (nearbyCount > 0)
        {
            // Alignment: average velocity
            foreach (var vel in nearbyYokaiVelocities)
                alignment += vel;
            alignment /= nearbyCount;

            // Cohesion: move toward average position
            Vector3 avgPosition = Vector3.zero;
            foreach (var pos in nearbyYokaiPositions)
                avgPosition += pos;
            avgPosition /= nearbyCount;
            cohesion = (avgPosition - yokaiPosition).normalized;

            // Separation: avoid crowding
            foreach (var pos in nearbyObstacles)
            {
                Vector3 toObj = yokaiPosition - pos;
                if (toObj.magnitude > 0f)
                    separation += toObj.normalized / toObj.magnitude;
            }

            alignment *= alignmentWeight;
            cohesion *= cohesionWeight;
            separation *= separationWeight;

            Vector3 boidDir = alignment + cohesion + separation;
            return boidDir.normalized;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Utility function to get nearby objects.
    /// </summary>
    public static List<Collider> GetNearby(Vector3 position, float detectionRadius, LayerMask layerMask)
    {
        Collider[] hits = Physics.OverlapSphere(position, detectionRadius, layerMask);
        return new List<Collider>(hits);
    }
}
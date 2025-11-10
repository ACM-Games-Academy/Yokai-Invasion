using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Profiling;

public static class Boids
{
    private static readonly ProfilerMarker boidMarker = new ProfilerMarker("Boids");

    private static float alignmentWeight = 1f;
    private static float cohesionWeight = 1f;
    private static float separationWeight = 1f;
    private static float obstacleAvoidWeight = 3f;
    private static float neighborhoodRadius = 3f;
    private static float separationRadius = 1.5f;

    [BurstCompile]
    private struct ComputeBoidJob : IJob
    {
        [ReadOnly] public NativeArray<float3> yokaiPositions;
        [ReadOnly] public NativeArray<float3> yokaiVelocities;
        [ReadOnly] public NativeArray<float3> obstaclePositions;

        public int boidIndex;
        public float alignmentWeight;
        public float cohesionWeight;
        public float separationWeight;
        public float obstacleAvoidWeight;
        public float neighborhoodRadius;
        public float separationRadius;

        public NativeArray<float3> outputDir;

        public void Execute()
        {
            float3 position = yokaiPositions[boidIndex];
            float3 velocity = yokaiVelocities[boidIndex];

            float3 alignment = float3.zero;
            float3 cohesion = float3.zero;
            float3 separation = float3.zero;

            int neighborCount = 0; // track how many neighbors we have

            for (int i = 0; i < yokaiPositions.Length; i++)
            {
                if (i == boidIndex) continue;

                float3 diff = position - yokaiPositions[i];
                float dist = math.length(diff);

                // --- Separation ---
                if (dist < separationRadius && dist > 0f)
                    separation += diff / dist;

                // --- Alignment & Cohesion ---
                if (dist < neighborhoodRadius)
                {
                    alignment += yokaiVelocities[i];
                    cohesion += yokaiPositions[i];
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                // --- Alignment ---
                alignment /= neighborCount;

                // --- Cohesion ---
                float3 averagePos = cohesion / neighborCount;
                cohesion = averagePos - position; // direction toward center
            }

            // normalize + weight
            separation = math.normalizesafe(separation) * separationWeight;
            alignment = math.normalizesafe(alignment) * alignmentWeight;
            cohesion = math.normalizesafe(cohesion) * cohesionWeight;

            float3 boidDir = alignment + cohesion + separation;

            if (math.lengthsq(boidDir) < 0.001f)
                boidDir = new float3(0f, 0f, 1f);

            outputDir[0] = math.normalizesafe(boidDir);
        }

    }

    public static Vector3 Path(
        Vector3 yokaiPosition,
        Vector3 yokaiVelocity,
        List<Vector3> nearbyYokaiPositions,
        List<Vector3> nearbyYokaiVelocities,
        List<Vector3> nearbyObstacles
    )
    {
        using (boidMarker.Auto())
        {
            int count = nearbyYokaiPositions.Count;

            var posArray = new NativeArray<float3>(count + 1, Allocator.TempJob);
            var velArray = new NativeArray<float3>(count + 1, Allocator.TempJob);
            var obsArray = new NativeArray<float3>(nearbyObstacles.Count, Allocator.TempJob);
            var outDir = new NativeArray<float3>(1, Allocator.TempJob);

            posArray[0] = yokaiPosition;
            velArray[0] = yokaiVelocity;

            for (int i = 0; i < count; i++)
            {
                posArray[i + 1] = nearbyYokaiPositions[i];
                velArray[i + 1] = nearbyYokaiVelocities[i];
            }

            for (int i = 0; i < nearbyObstacles.Count; i++)
                obsArray[i] = nearbyObstacles[i];

            var job = new ComputeBoidJob
            {
                yokaiPositions = posArray,
                yokaiVelocities = velArray,
                obstaclePositions = obsArray,
                boidIndex = 0,
                alignmentWeight = alignmentWeight,
                cohesionWeight = cohesionWeight,
                separationWeight = separationWeight,
                obstacleAvoidWeight = obstacleAvoidWeight,
                neighborhoodRadius = neighborhoodRadius,
                separationRadius = separationRadius,
                outputDir = outDir
            };

            job.Run(); // synchronous execution

            Vector3 result = outDir[0];

            // Dispose manually
            posArray.Dispose();
            velArray.Dispose();
            obsArray.Dispose();
            outDir.Dispose();

            return result;
        }
    }

    public static List<Collider> GetNearby(Vector3 position, float detectionRadius, LayerMask layerMask)
    {
        Collider[] hits = Physics.OverlapSphere(position, detectionRadius, layerMask);
        return new List<Collider>(hits);
    }
}

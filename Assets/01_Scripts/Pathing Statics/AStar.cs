using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Unity.Profiling;

public static class AStar
{
    private static readonly ProfilerMarker AStarMarker = new ProfilerMarker("A*");

    private static PathingManager pathingManager;

    public static void Initialize(PathingManager manager)
    {
        pathingManager = manager;
    }

    public static Vector3[] Path(Vector3 start, Vector3 end)
    {
        using (AStarMarker.Auto())
        {
            if (pathingManager == null)
            {
                Debug.LogError("AStar not initialized with PathingManager!");
                return new Vector3[] { start, end };
            }

            int2 startGridCoord = pathingManager.WorldToGrid(start);
            int2 endGridCoord = pathingManager.WorldToGrid(end);

            int gridSize = pathingManager.GetNodeMapSize();
            var nodeMap = pathingManager.GetNodeMap();

            // Allocate job data
            var cameFrom = new NativeArray<int>(nodeMap.Length, Allocator.TempJob);
            var gScore = new NativeArray<int>(nodeMap.Length, Allocator.TempJob);
            var openSet = new NativeList<int>(Allocator.TempJob);
            var pathResult = new NativeList<int>(Allocator.TempJob);

            for (int i = 0; i < gScore.Length; i++) gScore[i] = int.MaxValue;

            var job = new ComputeAStar
            {
                nodeMap = nodeMap,
                cameFrom = cameFrom,
                gScore = gScore,
                openSet = openSet,
                pathResult = pathResult,
                startPos = startGridCoord,
                endPos = endGridCoord,
                gridSize = gridSize
            };

            job.Run();

            // Convert the result to world positions
            List<Vector3> worldPath = new();
            for (int i = 0; i < pathResult.Length; i++)
            {
                int index = pathResult[i];
                int2 gridPos = new int2(index / gridSize, index % gridSize);
                worldPath.Add(pathingManager.GridToWorld(gridPos));
            }

            // Dispose
            cameFrom.Dispose();
            gScore.Dispose();
            openSet.Dispose();
            pathResult.Dispose();

            return worldPath.ToArray();
        }
    }

    [BurstCompile]
    private struct ComputeAStar : IJob
    {
        [ReadOnly] public NativeArray<PathingManager.PathNode> nodeMap;

        public NativeArray<int> cameFrom;
        public NativeArray<int> gScore;
        public NativeList<int> openSet;
        public NativeList<int> pathResult;

        public int2 startPos;
        public int2 endPos;
        public int gridSize;

        public void Execute()
        {
            int startIndex = Index(startPos.x, startPos.y);
            int endIndex = Index(endPos.x, endPos.y);

            gScore[startIndex] = 0;
            openSet.Add(startIndex);

            while (openSet.Length > 0)
            {
                // Pick node with lowest F cost
                int current = openSet[0];
                int lowestF = gScore[current] + Heuristic(current, endIndex);
                for (int i = 1; i < openSet.Length; i++)
                {
                    int nodeIndex = openSet[i];
                    int f = gScore[nodeIndex] + Heuristic(nodeIndex, endIndex);
                    if (f < lowestF)
                    {
                        current = nodeIndex;
                        lowestF = f;
                    }
                }

                if (current == endIndex)
                {
                    ReconstructPath(endIndex);
                    return;
                }

                RemoveFromOpenSet(current);
                int2 currentPos = Pos(current);

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        int2 neighbor = new int2(currentPos.x + dx, currentPos.y + dy);
                        if (!IsInsideGrid(neighbor)) continue;

                        int neighborIndex = Index(neighbor.x, neighbor.y);
                        if (!nodeMap[neighborIndex].isWalkable) continue;

                        int tentativeG = gScore[current] + ((dx == 0 || dy == 0) ? 10 : 14);

                        if (tentativeG < gScore[neighborIndex])
                        {
                            cameFrom[neighborIndex] = current;
                            gScore[neighborIndex] = tentativeG;
                            if (!openSet.Contains(neighborIndex))
                                openSet.Add(neighborIndex);
                        }
                    }
                }
            }
        }

        private void ReconstructPath(int endIndex)
        {
            int current = endIndex;
            var stack = new NativeList<int>(Allocator.Temp);

            while (cameFrom[current] != 0)
            {
                stack.Add(current);
                current = cameFrom[current];
                if (current == cameFrom[current]) break;
            }

            for (int i = stack.Length - 1; i >= 0; i--)
                pathResult.Add(stack[i]);

            stack.Dispose();
        }

        private int Index(int x, int y) => x * gridSize + y;
        private int2 Pos(int index) => new int2(index / gridSize, index % gridSize);
        private bool IsInsideGrid(int2 pos) => pos.x >= 0 && pos.y >= 0 && pos.x < gridSize && pos.y < gridSize;
        private int Heuristic(int a, int b)
        {
            int2 pa = Pos(a);
            int2 pb = Pos(b);
            int dx = math.abs(pa.x - pb.x);
            int dy = math.abs(pa.y - pb.y);
            return 10 * (dx + dy);
        }

        private void RemoveFromOpenSet(int value)
        {
            for (int i = 0; i < openSet.Length; i++)
            {
                if (openSet[i] == value)
                {
                    openSet.RemoveAtSwapBack(i);
                    return;
                }
            }
        }
    }
}

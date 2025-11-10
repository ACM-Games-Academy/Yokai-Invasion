using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Unity.Profiling;

/// <summary>
/// This manager is responsible for maintaining a global grind of nodes for pathfinding that represent walkable and non-walkable areas.
/// It will handle collision checks and updating the grid as necessary.
/// Finally, it will provide a pathfinding interface for other classes to request paths without themselves performing physics checks.
/// </summary>
public class PathingManager : MonoBehaviour
{
    private static readonly ProfilerMarker pathingManagerMarker = new ProfilerMarker("PathingManager");

    private const int NODE_SIZE = 1;
    private const int X_WORLD_OFFSET = -143; // Offsets such that the center of the node map is the center of the temple, (7, 0 ,-7)
    private const int Y_WORLD_OFFSET = -157;

    private const float WORLD_HEIGHT = 0.66f;

    private const int PLAYSPACE_SIZE = 300;

    private NativeArray<PathNode> nodeMap;

    public struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public int costFromStart;
        public int estimatedCostToEnd;
        public int totalCost => costFromStart + estimatedCostToEnd;

        public bool isWalkable;

        public int cameFromIndex;
    }

    private void Awake()
    {
        InitializeGrid();
        AStar.Initialize(this);
    }

    private void InitializeGrid()
    {
        using (pathingManagerMarker.Auto())
        {
            nodeMap = new NativeArray<PathNode>(PLAYSPACE_SIZE * PLAYSPACE_SIZE, Allocator.Persistent);

            for (int x = 0; x < PLAYSPACE_SIZE; x++)
            {
                for (int y = 0; y < PLAYSPACE_SIZE; y++)
                {
                    int index = x * PLAYSPACE_SIZE + y;
                    Vector3 worldPos = new Vector3(x * NODE_SIZE + X_WORLD_OFFSET, 0, y * NODE_SIZE + Y_WORLD_OFFSET);
                    bool walkable = !Physics.CheckBox(worldPos, Vector3.one * NODE_SIZE * 0.5f, Quaternion.identity, LayerMask.GetMask("Obstacle"));

                    nodeMap[index] = new PathNode
                    {
                        x = x,
                        y = y,
                        index = index,
                        isWalkable = walkable,
                        costFromStart = int.MaxValue,
                        estimatedCostToEnd = 0,
                        cameFromIndex = -1
                    };
                }
            }
        }
    }

    public int2 WorldToGrid(Vector3 worldPos)
    {
        using (pathingManagerMarker.Auto())
        {
            return new int2(
            Mathf.FloorToInt((worldPos.x - X_WORLD_OFFSET) / NODE_SIZE),
            Mathf.FloorToInt((worldPos.z - Y_WORLD_OFFSET) / NODE_SIZE)
);
        }
    }

    public Vector3 GridToWorld(int2 gridPos)
    {
        using (pathingManagerMarker.Auto())
        {
            return new Vector3(
            gridPos.x * NODE_SIZE + X_WORLD_OFFSET,
            WORLD_HEIGHT,
            gridPos.y * NODE_SIZE + Y_WORLD_OFFSET
        );
        }
    }


    private void OnDestroy()
    {
        if (nodeMap.IsCreated)
        {
            nodeMap.Dispose();
        }
    }

    // Getters

    public NativeArray<PathNode> GetNodeMap() => nodeMap;
    public int GetNodeMapSize() => PLAYSPACE_SIZE;
}

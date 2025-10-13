using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public class PathNode
    {
        public Vector2Int gridPos;
        public PathNode parent;
        public float G;
        public float H;
        public float F => G + H;

        public PathNode(Vector2Int gridPos, PathNode parent, float g, float h)
        {
            this.gridPos = gridPos;
            this.parent = parent;
            G = g;
            H = h;
        }
    }

    private static float worldHeight;
    private static float cellSize = 1.0f;
    private static float stopDistance = 0.5f;
    private static int maxIterations = 2500;

    // --- GIZMO DEBUGGING DATA ---
    public struct SphereCheck
    {
        public Vector3 pos;
        public bool hit;
    }

    public static readonly List<SphereCheck> gizmoChecks = new();
    public static readonly List<Vector3> gizmoPath = new();

    public static void ClearGizmoData()
    {
        gizmoChecks.Clear();
        gizmoPath.Clear();
    }

    // -----------------------------

    public static Vector3[] AStarPath(Vector3 start, Vector3 end)
    {
        ClearGizmoData(); // clear gizmo data each run
        worldHeight = start.y;

        Vector2Int startGrid = WorldToGrid(start);
        Vector2Int endGrid = WorldToGrid(end);

        List<PathNode> openList = new List<PathNode>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        PathNode startNode = new PathNode(startGrid, null, 0, Vector2Int.Distance(startGrid, endGrid));
        openList.Add(startNode);

        while (openList.Count > 0 && closedSet.Count < maxIterations)
        {
            PathNode current = openList[0];
            foreach (var node in openList)
            {
                if (node.F < current.F)
                    current = node;
            }

            openList.Remove(current);
            closedSet.Add(current.gridPos);

            if (current.gridPos == endGrid || Vector2Int.Distance(current.gridPos, endGrid) < stopDistance)
            {
                var path = ReconstructPath(current);
                gizmoPath.AddRange(path); // store green path positions
                return path;
            }

            foreach (Vector2Int neighborPos in GetNeighbors(current.gridPos))
            {
                if (closedSet.Contains(neighborPos)) continue;

                float tentativeG = current.G + Vector2Int.Distance(current.gridPos, neighborPos);

                PathNode neighbor = openList.Find(n => n.gridPos == neighborPos);
                if (neighbor == null)
                {
                    neighbor = new PathNode(
                        neighborPos,
                        current,
                        tentativeG,
                        Vector2Int.Distance(neighborPos, endGrid)
                    );
                    openList.Add(neighbor);
                }
                else if (tentativeG < neighbor.G)
                {
                    neighbor.G = tentativeG;
                    neighbor.parent = current;
                }
            }
        }

        return new Vector3[] { start, end };
    }

    private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int position)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int( 1,  0),
            new Vector2Int(-1,  0),
            new Vector2Int( 0,  1),
            new Vector2Int( 0, -1),
            new Vector2Int( 1,  1),
            new Vector2Int( 1, -1),
            new Vector2Int(-1,  1),
            new Vector2Int(-1, -1),
        };

        foreach (var direction in directions)
        {
            Vector2Int neighbor = position + direction;
            Vector3 worldPosition = GridToWorld(neighbor);

            bool hit = Physics.CheckSphere(worldPosition, cellSize, LayerMask.GetMask("Obstacle"));
            gizmoChecks.Add(new SphereCheck { pos = worldPosition, hit = hit });

            if (hit)
                continue;

            yield return neighbor;
        }
    }

    private static Vector3[] ReconstructPath(PathNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        PathNode current = endNode;
        while (current != null)
        {
            path.Add(GridToWorld(current.gridPos));
            current = current.parent;
        }
        path.Reverse();
        return path.ToArray();
    }

    private static Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.z / cellSize)
        );
    }

    private static Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * cellSize,
            worldHeight,
            gridPos.y * cellSize
        );
    }
}

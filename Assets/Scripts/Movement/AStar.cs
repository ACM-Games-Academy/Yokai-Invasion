using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class AStar : MonoBehaviour
{
    public class PathNode
    {
        public Vector2Int gridPos;
        public PathNode parent;
        public float G; // cost from start
        public float H; // heuristic to target
        public float F => G + H;

        public PathNode(Vector2Int gridPos, PathNode parent, float g, float h)
        {
            this.gridPos = gridPos;
            this.parent = parent;
            G = g;
            H = h;
        }
    }

    [SerializeField] 
    private float cellSize = 1.0f;

    [SerializeField]
    private float stopDistance = 0.5f;

    private int maxIterations = 1000;

    public Vector3[] AStarPath(Vector3 start, Vector3 end)
    {
        Vector2Int startGrid = WorldToGrid(start);
        Vector2Int endGrid = WorldToGrid(end);

        List<PathNode> openList = new List<PathNode>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        // Start node
        PathNode startNode = new PathNode(startGrid, null, 0, Vector2Int.Distance(startGrid, endGrid));
        openList.Add(startNode);

        while (openList.Count > 0 && closedSet.Count < maxIterations)
        {
            // 1. Get node with lowest F cost
            PathNode current = openList[0];
            foreach (var node in openList)
            {
                if (node.F < current.F)
                    current = node;
            }

            // 2. Move current from open to closed
            openList.Remove(current);
            closedSet.Add(current.gridPos);

            // 3. If reached end, reconstruct path
            // 3. Check if we reached the end
            if (current.gridPos == endGrid || Vector2Int.Distance(current.gridPos, endGrid) < stopDistance)
            {
                return ReconstructPath(current);
            }

            // 4. Get neighbors
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
                    // Found a better path
                    neighbor.G = tentativeG;
                    neighbor.parent = current;
                }
            }
        }
        // No path found
        return new Vector3[] { start, end };
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int position)
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

            float checkHeight = 5f; // max obstacle height
            Vector3 halfExtents = new Vector3(cellSize * 0.45f, checkHeight * 0.5f, cellSize * 0.45f);
            Vector3 center = worldPosition + new Vector3(0f, halfExtents.y, 0f);

            if (Physics.CheckBox(center, halfExtents, Quaternion.identity, LayerMask.GetMask("Obstacle")))
            {
                Debug.Log($"Obstacle at {neighbor}");
                continue;
            }


            yield return neighbor;
        }
    }

    private Vector3[] ReconstructPath(PathNode endNode)
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


    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.z / cellSize)
        );
    }

    private Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * cellSize,
            0.66f,
            gridPos.y * cellSize
        );
    }
}

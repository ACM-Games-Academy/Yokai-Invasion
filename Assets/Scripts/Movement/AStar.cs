using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class AStar : MonoBehaviour
{
    [SerializeField]
    private float stopDistance = 0.5f;

    private int maxIterations = 100;

    public Vector3[] AStarPath(Vector3 start, Vector3 end)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        // Start node
        PathNode startNode = new PathNode(start, null, 0, Vector3.Distance(start, end));
        openList.Add(startNode);

        while (openList.Count > 0 && closedList.Count < maxIterations)
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
            closedList.Add(current);

            // 3. If reached end, reconstruct path
            if (Vector3.Distance(current.Position, end) < stopDistance)
            {
                return ReconstructPath(current);
            }

            // 4. Get neighbors
            foreach (Vector3 neighborPos in GetNeighbors(current.Position))
            {
                if (closedList.Exists(n => n.Position == neighborPos))
                    continue;

                float tentativeG = current.G + Vector3.Distance(current.Position, neighborPos);

                PathNode neighbor = openList.Find(n => Vector3.Distance(n.Position, neighborPos) < 0.5);
                if (neighbor == null)
                {
                    neighbor = new PathNode(
                        neighborPos,
                        current,
                        tentativeG,
                        Vector3.Distance(neighborPos, end)
                    );
                    openList.Add(neighbor);
                }
                else if (tentativeG < neighbor.G)
                {
                    // Found a better path
                    neighbor.G = tentativeG;
                    neighbor.Parent = current;
                }
            }
        }
        // No path found
        return new Vector3[] { start, end };
    }

    private IEnumerable<Vector3> GetNeighbors(Vector3 position)
    {
        // Placeholder: Replace with actual neighbor retrieval logic
        float step = 1.0f;
        List<Vector3> possibleNeighbours = new List<Vector3>
        {
            position + new Vector3(step, 0, 0),
            position + new Vector3(-step, 0, 0),
            position + new Vector3(0, 0, step),
            position + new Vector3(0, 0, -step),
            position + new Vector3(step, 0, step),
            position + new Vector3(step, 0, -step),
            position + new Vector3(-step, 0, step),
            position + new Vector3(-step, 0, -step)
        };

        foreach (var neighbor in possibleNeighbours)
        {
            if (Physics.CheckSphere(neighbor, 0.4f, LayerMask.GetMask("Obstacle")))
                continue;

            yield return neighbor;
        }
    }

    private Vector3[] ReconstructPath(PathNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        PathNode current = endNode;
        while (current != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }
        path.Reverse();
        return path.ToArray();
    }
}

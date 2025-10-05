using UnityEngine;

public class PathNode
{
    public Vector3 Position;
    public PathNode Parent;
    public float G; // cost from start
    public float H; // heuristic to target
    public float F => G + H; // total cost

    public PathNode(Vector3 pos, PathNode parent, float g, float h)
    {
        Position = pos;
        Parent = parent;
        G = g;
        H = h;
    }
}

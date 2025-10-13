using UnityEngine;

public class AStarGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // --- Draw checked spheres ---
        foreach (var check in AStar.gizmoChecks)
        {
            Gizmos.color = check.hit ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(check.pos, 0.5f);
        }

        // --- Draw reconstructed path ---
        if (AStar.gizmoPath.Count > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < AStar.gizmoPath.Count - 1; i++)
            {
                Gizmos.DrawLine(AStar.gizmoPath[i], AStar.gizmoPath[i + 1]);
                Gizmos.DrawWireSphere(AStar.gizmoPath[i], 0.2f);
            }
            Gizmos.DrawWireSphere(AStar.gizmoPath[^1], 0.2f);
        }
    }
}

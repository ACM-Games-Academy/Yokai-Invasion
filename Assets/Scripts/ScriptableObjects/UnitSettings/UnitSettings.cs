using UnityEngine;

[CreateAssetMenu(fileName = "UnitSettings", menuName = "ScriptableObjects/UnitSettings")]
public class UnitSettings : ScriptableObject
{
    [SerializeField]
    private float waypointTolerance;
    [SerializeField]
    private float moveSpeed;

    public float WaypointTolerance => waypointTolerance;
    public float MoveSpeed => moveSpeed;
}

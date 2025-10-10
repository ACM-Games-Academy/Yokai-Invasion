using UnityEngine;

[CreateAssetMenu(fileName = "YokaiSettings", menuName = "ScriptableObjects/YokaiSettings", order = 1)]
public class YokaiSettings : ScriptableObject
{
    [Header("Yokai Stats")]

    [Tooltip("The type of Yokai.")]
    [SerializeField]
    private string yokaiName;

    [Tooltip("The speed at which the Yokai moves.")]
    [Range(0f, 5f)]
    [SerializeField]
    private float moveSpeed;

    [Tooltip("The maximum health of the Yokai.")]
    [Range(1, 100)]
    [SerializeField]
    private int maxHealth;

    [Tooltip("The attack power of the Yokai.")]
    [Range(1, 50)]
    [SerializeField]
    private int attackPower;

    [Header("Pathfinding Settings")]

    [Tooltip("How reliant the Yokai is on allies, low values emphasize A* pathfinding while high values emphasize boids.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float allyReliance;

    [Tooltip("The tolerance distance to waypoints when following an A* path.")]
    [Range(0.1f, 2f)]
    [SerializeField]
    private float waypointTolerance;

    [Tooltip("The radius within which the Yokai detects objects for boid behavior.")]
    [Range(1f, 20f)]
    [SerializeField]
    private float detectionRadius;

    [Tooltip("The degree of emphasis on cohesion in boid behavior.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float cohesionWeight;

    [Tooltip("The degree of emphasis on separation in boid behavior.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float separationWeight;

    [Tooltip("The degree of emphasis on alignment in boid behavior.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float alignmentWeight;

    [Header("Spawning Settings")]
    [Tooltip("The point value of this yokai.")]
    [Range(1, 10)]
    [SerializeField]
    private int pointValue;


    // --- Public Read-Only Properties ---

    public string YokaiName => yokaiName;
    public float MoveSpeed => moveSpeed;
    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;

    public float AllyReliance => allyReliance;
    public float WaypointTolerance => waypointTolerance;
    public float DetectionRadius => detectionRadius;

    public float CohesionWeight => cohesionWeight;
    public float SeparationWeight => separationWeight;
    public float AlignmentWeight => alignmentWeight;

    public int PointValue => pointValue;
}

using UnityEngine;

[CreateAssetMenu(fileName = "HeroSettings", menuName = "ScriptableObjects/HeroSettings")]
public class HeroSettings : ScriptableObject
{
    [Header("Hero Stats")]
    [Tooltip("The movement speed of the hero")]
    [SerializeField]
    private float movementSpeed = 5f;

    [Tooltip("The maximum health of the hero")]
    [SerializeField]
    private int maxHealth = 100;

    [Tooltip("The attack power of the hero")]
    [SerializeField]
    private int attackPower = 1;

    [Tooltip("The attack delay of the hero in seconds")]
    [SerializeField]
    private float attackDelay = 2f;

    [Tooltip("The attack range of the hero")]
    [SerializeField]
    private float attackRange = 3f;

    public float MovementSpeed => movementSpeed;
    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
}

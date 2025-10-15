using UnityEngine;

[CreateAssetMenu(fileName = "HeroSettings", menuName = "ScriptableObjects/HeroSettings")]
public class HeroSettings : ScriptableObject
{
    [Header("Hero Stats")]
    [Tooltip("The movement speed of the hero")]
    [SerializeField]
    private float movementSpeed;

    [Tooltip("The maximum health of the hero")]
    [SerializeField]
    private int maxHealth;

    [Tooltip("The attack power of the hero")]
    [SerializeField]
    private int attackPower;

    [Tooltip("The attack delay of the hero in seconds")]
    [SerializeField]
    private float attackDelay;

    [Tooltip("The attack range of the hero")]
    [SerializeField]
    private float attackRange;

    public float MovementSpeed => movementSpeed;
    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
}

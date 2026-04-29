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

    [Header("Starting Resources")]

    [Tooltip("The starting gold amount for the hero")]
    [SerializeField]
    private int startingGold;

    [Tooltip("The starting wood amount for the hero")]
    [SerializeField]
    private int startingWood;

    [Tooltip("The starting food amount for the hero")]
    [SerializeField]
    private int startingFood;

    public float MovementSpeed => movementSpeed;
    public int MaxHealth => maxHealth;
    public int StartingGold => startingGold;
    public int StartingWood => startingWood;
    public int StartingFood => startingFood;

}

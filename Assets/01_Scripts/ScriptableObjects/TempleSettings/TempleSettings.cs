using UnityEngine;

[CreateAssetMenu(fileName = "TempleSettings", menuName = "Scriptable Objects/TempleSettings")]
public class TempleSettings : ScriptableObject
{
    [Tooltip("The maximum health of the Temple.")]
    [SerializeField]
    private int maxHealth;

    public int MaxHealth => maxHealth;
}

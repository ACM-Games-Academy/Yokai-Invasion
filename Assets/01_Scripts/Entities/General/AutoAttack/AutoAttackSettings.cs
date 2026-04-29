using UnityEngine;

[CreateAssetMenu(fileName = "AutoAttackSettings", menuName = "ScriptableObjects/AutoAttackSettings")]
public class AutoAttackSettings : ScriptableObject
{
    [Header("Attack Conditions")]
    public float AttackRange;
    public float AttackDelay;
    public LayerMask LayerMask;

    [Header("Attack Effects")] 
    public string AttackSound;
    public int AttackPower;
}

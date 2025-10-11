using UnityEngine;

public interface Yokai : Damageable, AutoAttacker
{
    YokaiSettings yokaiSettings { get; }
    public void DetermineTarget();
}

using UnityEngine;

public interface IYokai : IDamageable
{
    YokaiSettings yokaiSettings { get; }

    public void DealDamage(int damageAmount);
    public void DetermineTarget();
}

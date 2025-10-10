using UnityEngine;

public interface IYokai
{
    YokaiSettings yokaiSettings { get; }

    public void TakeDamage(int damageAmount);
    public void DealDamage(int damageAmount);
    public void DetermineTarget();
    public void MoveTowardsTarget();
}

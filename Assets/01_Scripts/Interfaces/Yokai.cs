using UnityEngine;

public interface Yokai : Damageable, AutoAttacker
{
    YokaiSettings yokaiSettings { get; }
    public void DetermineTarget();

    public enum States
    {
        Idle,
        Attacking,
        Pursuing,
        Fleeing,
        Dead
    }

    public States GetCurrentState();
    public void SetState(States state);
}

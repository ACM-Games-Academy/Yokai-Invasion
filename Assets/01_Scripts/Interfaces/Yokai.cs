using UnityEngine;

public interface Yokai : Damageable, AutoAttacker
{
    YokaiSettings yokaiSettings { get; }
    public Vector3 DetermineTarget();

    public States state { get; }
    public enum States
    {
        Idle,
        Attacking,
        Pursuing,
        Fleeing,
        Dead
    }

    public void SetState(States state);
}

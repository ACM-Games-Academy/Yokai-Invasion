using UnityEngine;

public interface Yokai : Damageable
{
    YokaiSettings yokaiSettings { get; }

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

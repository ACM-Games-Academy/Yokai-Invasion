using UnityEngine;

public class YokaiState : MonoBehaviour
{
    public enum YokaiStates
    {
        Idle,
        Attacking,
        Pursuing,
        Fleeing,
        Dead
    }
    private YokaiStates currentState = YokaiStates.Idle;

    private int health = 2;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = YokaiStates.Dead;
            // Handle death logic here
            Die();
            Debug.Log("Yokai is dead.");
        }
        else
        {
            // Add morale check here
            currentState = YokaiStates.Fleeing;
            // Handle fleeing logic here
            Debug.Log("Yokai is fleeing.");
        }
    }

    public int GetHealth()
    {
        return health;
    }

    private void Die()
    {
        // Handle death logic here
        Debug.Log("Yokai has died.");
        Destroy(gameObject);
    }

    public YokaiStates GetCurrentState()
    {
        return currentState;
    }
}

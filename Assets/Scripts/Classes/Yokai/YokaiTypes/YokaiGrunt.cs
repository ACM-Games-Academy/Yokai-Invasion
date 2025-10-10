using UnityEngine;

public class YokaiGrunt : MonoBehaviour, IYokai
{
    [Header("Yokai Settings")]
    [Tooltip("Settings for the Yokai Grunt")]
    [SerializeField]
    private YokaiSettings yokaiSettings;
    YokaiSettings IYokai.yokaiSettings => yokaiSettings;

    [Header("Grunt Stats")]
    [Tooltip("The current health of the Yokai Grunt")]
    [SerializeField]
    private int currentHealth;

    private void Awake()
    {
        currentHealth = yokaiSettings.MaxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log($"{yokaiSettings.YokaiName} took {damageAmount} damage.");
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DealDamage(int damageAmount)
    {
        Debug.Log($"{yokaiSettings.YokaiName} dealt {damageAmount} damage.");
        // Implement damage dealing logic here
    }

    public void DetermineTarget()
    {
        Debug.Log($"{yokaiSettings.YokaiName} is determining its target.");
        // Implement target determination logic here
    }

    private void Die()
    {
        Debug.Log($"{yokaiSettings.YokaiName} has died.");
        // Implement death logic here (e.g., play animation, drop loot)
        ObjectPooler.Instance.ReturnPooledObject(gameObject);
    }
}

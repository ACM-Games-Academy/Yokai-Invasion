using UnityEngine;

public class YokaiGrunt : MonoBehaviour, Yokai
{
    [Header("Yokai Settings")]
    [Tooltip("Settings for the Yokai Grunt")]
    [SerializeField]
    private YokaiSettings yokaiSettings;
    YokaiSettings Yokai.yokaiSettings => yokaiSettings;

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

    public void AutoAttack()
    {
        Debug.Log($"{yokaiSettings.YokaiName} dealt {yokaiSettings.AttackPower} damage.");
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
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}

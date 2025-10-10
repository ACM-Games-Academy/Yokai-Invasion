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
        // Implement health reduction logic here
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

    public void MoveTowardsTarget()
    {
        Debug.Log($"{yokaiSettings.YokaiName} is moving towards its target.");
        // Implement movement logic here
    }
}

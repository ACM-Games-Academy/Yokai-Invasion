using NUnit.Framework;
using UnityEngine;

public class YokaiOni : MonoBehaviour, Yokai
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

    private Yokai.States state = Yokai.States.Idle;     // Exposing this to editor means it may fail to update
    Yokai.States Yokai.state => state;

    private void Awake()
    {
        Overseer.Instance.GetManager<NightCycle>().DawnStarted += OnDawn;
    }

    private void OnEnable()
    {
        currentHealth = yokaiSettings.MaxHealth;
        state = Yokai.States.Idle;
    }

    public void TakeDamage(int damageAmount)
    {
        //Debug.Log($"{yokaiSettings.YokaiName} took {damageAmount} damage.");
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // state = Yokai.States.Fleeing;
        }
    }

    public void SetState(Yokai.States newState) => state = newState;

    public void AutoAttack()
    {
        //Debug.Log($"{yokaiSettings.YokaiName} dealt {yokaiSettings.AttackPower} damage.");
        // Implement damage dealing logic here
    }

    public Vector3 DetermineTarget()
    {
        //Debug.Log($"{yokaiSettings.YokaiName} is determining its target.");
        // Implement target determination logic here

        

        return Vector3.zero;
    }

    private void Die()
    {
        // Debug.Log($"{yokaiSettings.YokaiName} has died.");
        // Implement death logic here (e.g., play animation, drop loot)

        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(yokaiSettings.DropAmount);
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }

    private void OnDawn()
    {
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}

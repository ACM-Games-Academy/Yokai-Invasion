 using UnityEngine;

public class YokaiGrunt : MonoBehaviour, Yokai
{
    [Header("Yokai Settings")]
    [Tooltip("Settings for the Yokai Grunt")]
    [SerializeField]
    private YokaiSettings yokaiSettings;
    YokaiSettings Yokai.yokaiSettings => yokaiSettings;

    private AudioSettings audioSettings;

    [Header("Grunt Stats")]
    [Tooltip("The current health of the Yokai Grunt")]
    private int currentHealth;

    private Yokai.States state = Yokai.States.Idle;     // Exposing this to editor means it may fail to update
    Yokai.States Yokai.state => state;

    private Collider[] targetsInRange;
    private float lastAttackTime;


    private void OnEnable()
    {
        currentHealth = yokaiSettings.MaxHealth;
    }
    private void Update()
    {
        DetermineTarget();
    }

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }

    public void TakeDamage(int damageAmount)
    {
        //  [11] Play_Damage_Yokai - Plays damage sound without armour scrape
        audioSettings.Events[11].Post(gameObject);

        //Debug.Log($"{yokaiSettings.YokaiName} took {damageAmount} damage.");
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            state = Yokai.States.Fleeing;
        }
    }

    public void SetState(Yokai.States newState) => state = newState;

    public void AutoAttack()
    {
        foreach (var targetCollider in targetsInRange)
        {
            var target = targetCollider.gameObject.GetComponent<Damageable>();

            if (target == null) continue;

            if(target == targetCollider.gameObject.GetComponent<Yokai>()) { return; }

            target.TakeDamage(yokaiSettings.AttackPower);
            //Debug.Log("An Ashigaru attacked a Yokai! They lost " + attackPower + "health!");
        }
        lastAttackTime = Time.time;
    }

    public void DetermineTarget()
    {
        //Debug.Log($"{yokaiSettings.YokaiName} is determining its target.");
        targetsInRange = Boids.GetNearby(transform.position, yokaiSettings.AttackRange, ~LayerMask.GetMask("Floor")).ToArray();

        if (targetsInRange.Length > 0 && Time.time >= lastAttackTime + yokaiSettings.AttackDelay)
        {
            AutoAttack();
        }
    }

    private void Die()
    {
        // Debug.Log($"{yokaiSettings.YokaiName} has died.");
        // Implement death logic here (e.g., play animation, drop loot)

        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(yokaiSettings.DropAmount);
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}

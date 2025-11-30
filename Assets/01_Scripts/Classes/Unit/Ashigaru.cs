using UnityEngine;

public class Ashigaru : SelectableUnit, AutoAttacker, Damageable
{
    private AudioSettings audioSettings;
    
    private Collider[] targetsInRange;
    private float lastAttackTime;
    private int currentHealth;

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;

        currentHealth = settings.TotalHealth;
    }
    private void Update()
    {
        targetsInRange = Boids.GetNearby(transform.position, settings.AttackRange, ~LayerMask.GetMask("Floor")).ToArray();

        if (targetsInRange.Length > 0 && Time.time >= lastAttackTime + settings.AttackDelay)
        {
            AutoAttack();
        }
    }

    public void AutoAttack()
    {
        foreach (var targetCollider in targetsInRange)
        {
            var target = targetCollider.gameObject.GetComponent<Yokai>();

            if (target == null) continue;

            target.TakeDamage(settings.AttackPower);
            //Debug.Log("An Ashigaru attacked a Yokai! They lost " + attackPower + "health!");
        }
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int damageAmount)
    {
        //  [17] Play_Damage_Ashigaru - Plays base damage sound and armour scraping
        audioSettings.Events[17].Post(gameObject);

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currentHealth = settings.TotalHealth;
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}

using UnityEngine;

public class Ashigaru : SelectableUnit, AutoAttacker, Damageable
{
    private Collider[] targetsInRange;
    private float lastAttackTime;
    private int currentHealth;

    private void Start()
    {
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
        currentHealth -= damageAmount;
        Debug.Log ("Ashigaru Health: "+ currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("ded");
        currentHealth = settings.TotalHealth;
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}

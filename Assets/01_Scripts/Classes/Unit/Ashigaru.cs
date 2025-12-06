using UnityEngine;

public class Ashigaru : SelectableUnit, AutoAttacker, Damageable
{
    private Collider[] targetsInRange;
    private float lastAttackTime;

    private int currentHealth;

    public Animator anim;

    private void Start()
    {
        base.Start();

        base.audioSettings = Overseer.Instance.Settings.AudioSettings;

        currentHealth = settings.TotalHealth;

        var nightCycle = Overseer.Instance.GetManager<NightCycle>();

        nightCycle.NightStarted += SetToNight;

        nightCycle.DawnStarted += SetToDay;
        nightCycle.DayStarted += SetToDay;
        nightCycle.DuskStarted += SetToDay;
    }

    private void SetToNight()
    {
        anim.SetBool("isNight", true);
    }

    private void SetToDay()
    {
        anim.SetBool("isNight", false);
    }

    private void Update()
    {
        targetsInRange = Boids.GetNearby(transform.position, settings.AttackRange, ~LayerMask.GetMask("Floor")).ToArray();

        if (targetsInRange.Length > 0 && Time.time >= lastAttackTime + settings.AttackDelay)
        {

            AutoAttack();
        }

        if (isWalking == true) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);


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
        //  [17] Play_Damage_Ashigaru - Plays base damage sound plus armour scraping
        audioSettings.Events[17].Post(gameObject);

        currentHealth -= damageAmount;
        //Debug.Log("Ashigaru Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //  [26] Play_Death_Ashigaru - Plays human death voice
        audioSettings.Events[26].Post(gameObject);

        currentHealth = settings.TotalHealth;
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}

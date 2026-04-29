using UnityEngine;

public class Ashigaru : Soldier, Damageable
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
        base.Update();

        if (isWalking == true) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);


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

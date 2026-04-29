using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour, Damageable
{
    private Collider[] yokaiInRange;
    private float lastAttackTime;
    private int currentHealth;

    public Action HeroTookDamage;

    public Animator animator;

    private AudioSettings audioSettings;

    [SerializeField] private HeroSettings heroSettings;

    public int CurrentHealth => currentHealth;

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
        
        currentHealth = heroSettings.MaxHealth;

        StartCoroutine(GiveStartingResources());

        var nightCycle = Overseer.Instance.GetManager<NightCycle>();
        nightCycle.DawnStarted += ResetHealth;
    }

    private IEnumerator GiveStartingResources()
    {
        yield return new WaitForSeconds(1);
        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(heroSettings.StartingGold);
        Overseer.Instance.GetManager<ResourceManager>().IncreaseWood(heroSettings.StartingWood);
        Overseer.Instance.GetManager<ResourceManager>().IncreaseFood(heroSettings.StartingFood);
    }

    private void Update()
    {
        yokaiInRange = Boids.GetNearby(transform.position, heroSettings.AttackRange, ~LayerMask.GetMask("Floor")).ToArray();

        if (yokaiInRange.Length > 0 && Time.time >= lastAttackTime + heroSettings.AttackDelay)
        {
            Attack();
        }
    }

    public void Attack()
    {
        foreach (var yokaiCollider in yokaiInRange)
        {
            var yokai = yokaiCollider.gameObject.GetComponent<Yokai>();

            if (yokai == null) continue;

            yokai.TakeDamage(heroSettings.AttackPower);
            animator.SetTrigger("Attack");

            Debug.Log("Attacked a Yokai! They lost " + heroSettings.AttackPower + "health!");
        }
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        HeroTookDamage?.Invoke();
        animator.SetTrigger("Hit");

        //  [17] Play_Damage_Ashigaru - Plays base damage sound plus armour scraping
        audioSettings.Events[17].Post(gameObject);

        if (currentHealth <= 0)
        {
            HeroMovement.GameOver();
            animator.SetBool("Dead", true);
            currentHealth = 0;
        }
    }

    private void ResetHealth()
    {
        currentHealth = heroSettings.MaxHealth;
    }
}

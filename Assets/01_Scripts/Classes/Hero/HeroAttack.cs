using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour, Damageable
{
    private Collider[] yokaiInRange;
    private float lastAttackTime;
    private int currentHealth;

    public Action HeroTookDamage;
    public Action HeroDead;

    public Animator animator;

    [SerializeField] private HeroSettings heroSettings;

    public int CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = heroSettings.MaxHealth;

        var nightCycle = Overseer.Instance.GetManager<NightCycle>();
        nightCycle.DawnStarted += ResetHealth;
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

        if (currentHealth <= 0)
        {
            animator.SetBool("Dead", true);
            Die();
        }
    }

    public void Die()
    {
        
        HeroDead?.Invoke();
        Debug.LogWarning("GAME OVER!");
        Time.timeScale = 0;
    }

    private void ResetHealth()
    {
        currentHealth = heroSettings.MaxHealth;
    }
}

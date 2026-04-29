using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats : MonoBehaviour, Damageable
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

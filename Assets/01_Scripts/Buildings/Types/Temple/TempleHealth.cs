using System;
using UnityEngine;

public class TempleHealth : MonoBehaviour, Damageable
{
    private TempleSettings settings;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    public Action TempleTookDamage;

    private GameObject hero;
    private CallDeath callDeath;
    private void Start()
    {
        settings = Overseer.Instance.Settings.TempleSettings;
        currentHealth = settings.MaxHealth;

        hero = GameObject.Find("Coloured Hero");
        callDeath = hero.GetComponent<CallDeath>();

        Overseer.Instance.GetManager<NightCycle>().DawnStarted += ResetHealth;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        TempleTookDamage?.Invoke();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    private void ResetHealth()
    {
        currentHealth = settings.MaxHealth;
    }

    private void GameOver()
    {
        callDeath.Die();
        currentHealth = 0;
    }
}

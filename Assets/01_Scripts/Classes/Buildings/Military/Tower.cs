using System.Linq;
using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class Tower : MilitaryBuilding, AutoAttacker
{
    private int garrisonedUnits = 0;
    private float attackRecovery = 1f;
    private float lastAttackTime;
    private float attackRange = 20f;
    private int damage = 10;

    private List<Yokai> yokaiInRange;

    private void Awake()
    {
        yokaiInRange = new List<Yokai>();
    }

    public override void Garrison(Soldier soldier)
    {
        Debug.Log($"{soldier} garrisoned {this.name}");
        soldier.gameObject.SetActive(false);

        garrisonedUnits++;
    }

    private void Update()
    {

        var colliderInRange = Boids.GetNearby(transform.position, attackRange, ~LayerMask.GetMask("Floor")).ToArray();

        foreach (Collider collider in colliderInRange)
        {
            var yokai = collider.gameObject.GetComponent<Yokai>();
            if (yokai != null)
            {
                yokaiInRange.Add(yokai);
            }
        }

        if (yokaiInRange.Count() > 0 && Time.time > lastAttackTime + attackRecovery)
        {
            AutoAttack();
        }

        yokaiInRange.Clear();
    }

    public void AutoAttack()
    {
        for (int i = 0; i < garrisonedUnits; i++)
        {
            var yokai = yokaiInRange[0];
            yokai.TakeDamage(damage);
            // Sound nice and shit
        }

        lastAttackTime = Time.time;
    }
}
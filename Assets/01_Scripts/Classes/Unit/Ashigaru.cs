using UnityEngine;

public class Ashigaru : Soldier, AutoAttacker
{
    private Collider[] targetsInRange;
    private float lastAttackTime;

    private float attackRange = 5f;
    private float attackDelay = 0.5f;
    private int attackPower = 2;

    private void Update()
    {
        base.Update();
        targetsInRange = Boids.GetNearby(transform.position, attackRange, ~LayerMask.GetMask("Floor")).ToArray();

        if (targetsInRange.Length > 0 && Time.time >= lastAttackTime + attackDelay)
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

            target.TakeDamage(attackPower);
            //Debug.Log("An Ashigaru attacked a Yokai! They lost " + attackPower + "health!");
        }
        lastAttackTime = Time.time;
    }
}

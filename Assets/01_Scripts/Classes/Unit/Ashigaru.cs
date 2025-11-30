using UnityEngine;

public class Ashigaru : SelectableUnit, AutoAttacker
{
    private Collider[] targetsInRange;
    private float lastAttackTime;

    private float attackRange = 5f;
    private float attackDelay = 0.5f;
    private int attackPower = 2;

    public Animator anim;

    private void Start()
    {
        base.Start();
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
        targetsInRange = Boids.GetNearby(transform.position, attackRange, ~LayerMask.GetMask("Floor")).ToArray();

        if (targetsInRange.Length > 0 && Time.time >= lastAttackTime + attackDelay)
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

            target.TakeDamage(attackPower);
            //Debug.Log("An Ashigaru attacked a Yokai! They lost " + attackPower + "health!");
        }
        lastAttackTime = Time.time;
    }
}

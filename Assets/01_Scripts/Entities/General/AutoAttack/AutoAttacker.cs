using System;
using UnityEngine;

public class AutoAttacker : MonoBehaviour
{
    public AutoAttackSettings settings;
    
    private Collider[] targetsInRange;
    private float lastAttackTime;
    
    public void DetermineTarget()
    {
        targetsInRange = Boids.GetNearby(transform.position, settings.AttackRange, settings.LayerMask).ToArray();

        if (targetsInRange.Length > 0 
            && Time.time >= lastAttackTime + settings.AttackDelay)
        {
            AutoAttack();
        }
    }
    
    public void AutoAttack()
    {
        AkUnitySoundEngine.PostEvent(settings.AttackSound, gameObject);
        
        var targetCollider = targetsInRange[0];
        var target = targetCollider.gameObject.GetComponent<Damageable>();

        target.TakeDamage(settings.AttackPower);
        lastAttackTime = Time.time;
    }

    private void Start()
    {
        lastAttackTime = Time.time;
    }

    private void Update()
    {
        DetermineTarget();
    }
}

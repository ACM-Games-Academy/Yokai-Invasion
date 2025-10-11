using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private Collider[] yokaiInRange;
    private float lastAttackTime;

    [SerializeField] private HeroSettings heroSettings;

    private void Update()
    {
        yokaiInRange = GetNearby(heroSettings.AttackRange, transform.position);

        if (yokaiInRange.Length > 0 && Time.time >= lastAttackTime + heroSettings.AttackDelay)
        {
            Attack();
        }
    }

    private Collider[] GetNearby(float range, Vector3 position)
    {
        int mask = ~LayerMask.GetMask("Floor"); // all layers EXCEPT floor

        Collider[] hits = Physics.OverlapSphere(position, range, mask);

        return hits;
    }

    public void Attack()
    {
        foreach (var yokaiCollider in yokaiInRange)
        {
            var yokai = yokaiCollider.gameObject.GetComponent<Yokai>();

            if (yokai == null) continue;

            yokai.TakeDamage(heroSettings.AttackPower);
            Debug.Log("Attacked a Yokai! They lost " + heroSettings.AttackPower + "health!");
        }
        lastAttackTime = Time.time;
    }
}

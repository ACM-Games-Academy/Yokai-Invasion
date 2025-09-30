using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private List<Collider> yokaiInRange = new List<Collider>();
    private List<Collider> yokaiToRemove = new List<Collider>();
    [SerializeField] private int attackDelay = 2; 
    private int attackDamage = 1;
    private float lastAttackTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Yokai") && !yokaiInRange.Contains(other))
        {
            yokaiInRange.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Yokai"))
        {
            yokaiInRange.Remove(other);
        }
    }

    public void Attack()
    {
        foreach (var yokaiCollider in yokaiInRange)
        {
            var yokai = yokaiCollider.gameObject.GetComponent<YokaiState>();

            if (yokai == null) continue;

            yokai.TakeDamage(attackDamage);
            Debug.Log("Attacked a Yokai! They now have " + yokai.GetHealth() + "health left!");

            if (yokai.GetHealth() <= 0)
            {
                yokaiToRemove.Add(yokaiCollider);
            }
        }
        foreach (var yokaiCollider in yokaiToRemove)
        {
            yokaiInRange.Remove(yokaiCollider);
        }
        yokaiToRemove.Clear();
        lastAttackTime = Time.time;
    }

    private void Update()
    {
        if (yokaiInRange.Count > 0 && Time.time >= lastAttackTime + attackDelay)
        {
            Attack();
        }
    }
}

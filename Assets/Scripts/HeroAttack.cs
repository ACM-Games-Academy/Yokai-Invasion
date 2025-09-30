using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private List<Collider> yokaiInRange = new List<Collider>();
    private int attackDamage = 1;

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
            if (yokaiCollider == null)
            {
                yokaiInRange.Remove(yokaiCollider);
            }

            var yokai = yokaiCollider.gameObject.GetComponent<YokaiState>();

            if (yokai == null) continue;

            yokai.TakeDamage(attackDamage);
            Debug.Log("Attacked a Yokai! They now have " + yokai.GetHealth() + "health left!");
        }
    }

    private void Update()
    {
        Debug.Log($"Yokai in range: {yokaiInRange.Count}");
        Attack();
    }
}

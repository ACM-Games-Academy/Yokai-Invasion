using UnityEngine;

public abstract class Building : MonoBehaviour, Damageable
{
    private BuildingState state;
    private enum BuildingState : byte
    {
        moving,
        constructing,
        functioning,
        destroyed
    }

    //physics.overlapcheckbox 
    //make a list of all selectable units in space 
    //use SetDestination to walk outside of collision area
    //wait until unites leave
    //turn collider from trigger to collider

    //make another abstract class that inherits from this - assignable building (hold a unit)

    private int health;


    public void TakeDamage (int damage)
    {

    }
}

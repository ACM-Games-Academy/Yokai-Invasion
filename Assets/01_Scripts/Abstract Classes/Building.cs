using System;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Building : MonoBehaviour, Damageable
{
    private BuildingState state;

    public BuildingSettings settings;
    private enum BuildingState : byte
    {
        moving,
        constructing,
        functioning,
        destroyed
    }

    private int currentHealth;

    //physics.overlapcheckbox 
    //make a list of all selectable units in space 
    //use SetDestination to walk outside of collision area
    //wait until unites leave
    //turn collider from trigger to collider

    //make another abstract class that inherits from this - assignable building (hold a unit)



    public void BuildingStartUp()
    {
        Debug.Log("fuck you");
        settings = Overseer.Instance.Settings.BuildingSettings;

        string key = this.GetPrefabDefinition().name;
        int index = Overseer.Instance.GetManager<BuildingSpawner>().IndexDictionary[key];
        currentHealth = settings.BuildingOptions[index].BuildingHealth;
        Debug.Log($"PREFAB: {this.GetPrefabDefinition().name}  Current Health = {currentHealth}");

    }

    private void AlterStates ()
    {
        switch (state)
        {
            case BuildingState.moving:
                this.GetComponent<BoxCollider>().enabled = false;
                break;
            case BuildingState.constructing:
                this.GetComponent<BoxCollider>().enabled = true;
                break;
            case BuildingState.functioning:
                this.GetComponent<BoxCollider>().enabled = true;
                break;
            case BuildingState.destroyed:
                this.GetComponent<BoxCollider>().enabled = false;
                break;
        }
    }

    public void TakeDamage (int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) { state = BuildingState.destroyed; }
    }
}

using System;
using System.Collections;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public abstract class Building : MonoBehaviour, Damageable
{
    public BuildingState State;

    public Action MovingState;
    public Action ConstructingState;
    public Action FunctioningState;
    public Action DestroyedState;

    public BuildingSettings settings;
    private AudioSettings audioSettings;
    public enum BuildingState : byte
    {
        moving,
        constructing,
        functioning,
        destroyed
    }

    private int currentHealth;
    private int index;
    private Vector3 fullSize;

    //physics.overlapcheckbox 
    //make a list of al l selectable units in space 
    //use SetDestination to walk outside of collision area
    //wait until unites leave
    //turn collider from trigger to collider

    //make another abstract class that inherits from this - assignable building (hold a unit)



    public void Start()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;
        audioSettings = Overseer.Instance.Settings.AudioSettings;

        string key = this.name;
        index = Overseer.Instance.GetManager<BuildingSpawner>().IndexDictionary[key];
        currentHealth = settings.BuildingOptions[index].BuildingHealth;
    }

    public void AlterState (BuildingState targetState)
    {
        switch (targetState)
        {
            case BuildingState.moving:
                Moving();
                break;
            case BuildingState.constructing:
                Constructing();
                break;
            case BuildingState.functioning:
                Functioning();
                break;
            case BuildingState.destroyed:
                Destroyed();
                break;
        }
    }

    public void Moving()
    {
        State = BuildingState.moving;
        this.GetComponent<BoxCollider>().enabled = false;
    }

    private void Constructing()
    {
        
        var boxCollider = this.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        fullSize = boxCollider.size;
        boxCollider.isTrigger = false;
        boxCollider.size = new Vector3 (0f, fullSize.y, 0f);

        StartCoroutine(BeingConstructed());
        AlterState(BuildingState.functioning);
    }

    private IEnumerator BeingConstructed()
    {
        float counter = 0;
        while (counter < settings.BuildingOptions[index].BuildTime)
        {
            counter += Time.deltaTime;
            this.GetComponent<BoxCollider>().size = Vector3.Lerp(
                                                                new Vector3 (0,fullSize.y,0), 
                                                                fullSize, 
                                                                counter / settings.BuildingOptions[index].BuildTime);
            yield return null;
        }

        
    }

    private void Functioning()
    {

    }

    private void Destroyed ()
    {

    }
    public void TakeDamage (int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) { AlterState(BuildingState.destroyed); }
    }
}

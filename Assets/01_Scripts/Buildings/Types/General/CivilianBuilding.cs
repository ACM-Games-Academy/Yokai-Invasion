using System.Collections;
using UnityEngine;

public abstract class CivilianBuilding : AssignableBuilding, ResourceGenerator
{
    public float GenerationCooldown { get; protected set; } = 5f;

    private void Start()
    {
        base.Start();
        
        Employees = new EmployableUnit[base.MaxEmployees];
        Overseer.Instance.GetManager<EmploymentManager>().NewEmployer(this);

        //Building.BuildingStartUp(); //fix this
    }

    public virtual int GetProduction()
    {
        return 0;
    }

    public virtual void GenerateResource(int amount)
    {
        // Default implementation does nothing
    }
}

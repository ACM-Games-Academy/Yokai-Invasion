using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Soldier : SelectableUnit
{
#nullable enable
    public MilitaryBuilding? CurrentEmployer;
#nullable disable
    private EmploymentManager employmentManager;

    private void Start()
    {
        employmentManager = Overseer.Instance.GetManager<EmploymentManager>();
        employmentManager.NewUnassignedSoldier(this);
    }

    public void OnAssignment(MilitaryBuilding employer)
    {
        //Debug.Log($"{name} has been employed at {employer.name}.");
        CurrentEmployer = employer;
        SetDestination(employer.transform.position);
    }

    public void OnFire()
    {
        //Debug.Log($"{name} has been fired from {CurrentEmployer?.name}.");
        CurrentEmployer = null;
    }

    protected void FindEmployer()
    {
        if (CurrentEmployer != null) return;

        employmentManager.FindEmployerForSoldier(this);
    }

    protected void Update()
    {
        if (CurrentEmployer == null) return;
        if (!isAtWorkplace()) return;

        CurrentEmployer.Garrison(this);
    }

    private bool isAtWorkplace()
    {
        if (CurrentEmployer == null) return false;

        float distanceToEmployer = Vector3.Distance(transform.position, CurrentEmployer.transform.position);

        if (distanceToEmployer <= CurrentEmployer.EmploymentRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
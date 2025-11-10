using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class EmployableUnit : Unit
{
    private float currentGenerationCooldown = 0f;

#nullable enable
    public CivilianBuilding? CurrentEmployer;
#nullable disable
    private EmploymentManager employmentManager;

    private void Start()
    {
        employmentManager = Overseer.Instance.GetManager<EmploymentManager>();
        employmentManager.NewEmployableUnit(this);
    }

    public void OnEmploy(CivilianBuilding employer)
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

        employmentManager.FindEmployerForUnit(this);
    }

    private void Update()
    {
        if (CurrentEmployer == null) return;
        if (!isAtWorkplace()) return;

        if (currentGenerationCooldown > 0f)
        {
            currentGenerationCooldown -= Time.deltaTime;
            return;
        }

        CurrentEmployer.GenerateResource(CurrentEmployer.GetProduction());
        //Debug.Log($"{name} generated {CurrentEmployer.GetProduction()} resources at {CurrentEmployer?.name}.");

        currentGenerationCooldown = CurrentEmployer.GenerationCooldown;

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

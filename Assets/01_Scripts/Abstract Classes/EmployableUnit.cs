using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class EmployableUnit : MonoBehaviour
{
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
        Debug.Log($"{name} has been employed at {employer.name}.");
        CurrentEmployer = employer;
        GoToWork();
    }

    public void OnFire()
    {
        Debug.Log($"{name} has been fired from {CurrentEmployer?.name}.");
        CurrentEmployer = null;
    }

    protected void GoToWork()
    {
        if (CurrentEmployer == null) return;
        
        AStar.Path(transform.position, CurrentEmployer.transform.position);
    }

    protected void FindEmployer()
    {
        if (CurrentEmployer != null) return;

        employmentManager.FindEmployerForUnit(this);
    }
}

using System.Collections;
using UnityEngine;

public abstract class CivilianBuilding : Building, Employer, ResourceGenerator
{
    public int MaxEmployees { get; } = 3;
    public EmployableUnit[] Employees { get; set; }

    public float GenerationCooldown { get; protected set; } = 5f;
    public float EmploymentRadius { get; protected set; } = 5f;

    private void Start()
    {
        Employees = new EmployableUnit[MaxEmployees];
        Overseer.Instance.GetManager<EmploymentManager>().NewEmployer(this);
    }

    public void NewEmployee(EmployableUnit unit)
    {
        for (int i = 0; i < Employees.Length; i++)
        {
            if (Employees[i] == null)
            {
                Employees[i] = unit;
                return;
            }
        }
    }

    public int CurrentEmployeeCount()
    {
        int count = 0;
        foreach (var employee in Employees)
        {
            if (employee != null) count++;
        }
        return count;
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

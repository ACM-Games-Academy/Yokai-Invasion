using System.Collections;
using UnityEngine;

public class CivilianBuilding : Building, Employer
{
    public int MaxEmployees { get; } = 3;
    public EmployableUnit[] Employees { get; set; }

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
}

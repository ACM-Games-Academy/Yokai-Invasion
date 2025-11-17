using UnityEngine;

public abstract class AssignableBuilding : Building, Employer
{
    public int MaxEmployees { get; } = 3;
    public EmployableUnit[] Employees { get; set; }

    public float EmploymentRadius { get; protected set; } = 5f;

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

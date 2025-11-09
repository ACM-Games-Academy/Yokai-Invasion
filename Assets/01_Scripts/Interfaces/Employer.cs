using UnityEngine;

public interface Employer
{
    int MaxEmployees { get; }
    EmployableUnit[] Employees { get; set; }

}


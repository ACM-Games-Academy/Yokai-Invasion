using System.Collections.Generic;
using UnityEngine;

public class EmploymentManager : MonoBehaviour
{
    public Dictionary<CivilianBuilding, EmployableUnit[]> EmployedUnits = new Dictionary<CivilianBuilding, EmployableUnit[]>();
    public Dictionary<MilitaryBuilding, Soldier[]> AssignedSoldiers = new Dictionary<MilitaryBuilding, Soldier[]>();

    public List<EmployableUnit> UnemployedUnits = new List<EmployableUnit>();
    public List<Soldier> UnassignedSoldiers = new List<Soldier>();

    public void NewEmployer(CivilianBuilding employer)
    {
        EmployedUnits[employer] = new EmployableUnit[employer.MaxEmployees];
        //Debug.Log($"New employer added: {employer.name}. Total employers: {EmployedUnits.Count}");

        foreach (var unit in UnemployedUnits.ToArray())
        {
            FindEmployerForUnit(unit);
        }
    }

    public void NewMilitaryEmployer(MilitaryBuilding employer)
    {
        AssignedSoldiers[employer] = new Soldier[employer.MaxEmployees];
        //Debug.Log($"New military employer added: {employer.name}. Total employers: {EmployedUnits.Count}");

        foreach (var unit in UnassignedSoldiers.ToArray())
        {
            FindEmployerForSoldier(unit);
        }
    }

    public void NewEmployableUnit(EmployableUnit unit)
    {
        UnemployedUnits.Add(unit);
        //Debug.Log($"New employable unit added: {unit.name}. Total unemployed units: {UnemployedUnits.Count}");
        FindEmployerForUnit(unit);
    }

    public void NewUnassignedSoldier(Soldier soldier)
    {
        UnassignedSoldiers.Add(soldier);

        FindEmployerForSoldier(soldier);
    }

    public void FindEmployerForUnit(EmployableUnit unit)
    {
        foreach (var employer in EmployedUnits.Keys)
        {
            if (HasVacancy(employer))
            {
                Employ(employer, unit);
                return;
            }
        }
        //Debug.Log($"{unit.name} could not find an employer and remains unemployed.");
    }

    public void FindEmployerForSoldier(Soldier soldier)
    {
        foreach (var employer in AssignedSoldiers.Keys)
        {
            if (HasMilitaryVacancy(employer))
            {
                Assign(employer, soldier);
                return;
            }
        }
        //Debug.Log($"{unit.name} could not find an employer and remains unemployed.");
    }

    public bool HasMilitaryVacancy(MilitaryBuilding employer)
    {
        if (!MilitaryEmployerExists(employer)) return false;

        foreach (var employee in AssignedSoldiers[employer])
        {
            if (employee == null) return true;
        }
        return false;
    }

    public bool MilitaryEmployerExists(MilitaryBuilding employer)
    {
        return AssignedSoldiers.ContainsKey(employer);
    }

    public void Assign(MilitaryBuilding employer, Soldier soldier)
    {
        if (IsAssigned(soldier)) return;

        int index = FindMilitaryVacancyIndex(employer);
        if (index == -1) return;

        UnassignedSoldiers.Remove(soldier);

        AssignedSoldiers[employer][index] = soldier;
        employer.NewSoldier(soldier);
        soldier.OnAssignment(employer);

        //Debug.Log($"Employed {unit.name}. {employer} now has {employer.CurrentEmployeeCount()} employees!");
    }

    public bool IsAssigned(Soldier soldier)
    {
        return UnassignedSoldiers.Contains(soldier) == false;
    }

    public int FindMilitaryVacancyIndex(MilitaryBuilding employer)
    {
        if (!MilitaryEmployerExists(employer)) return -1;

        for (int i = 0; i < AssignedSoldiers[employer].Length; i++)
        {
            if (AssignedSoldiers[employer][i] == null) return i;
        }
        return -1;
    }

    public void Employ(CivilianBuilding employer, EmployableUnit unit)
    {
        if (IsEmployed(unit)) return;

        int index = FindVacancyIndex(employer);
        if (index == -1) return;

        UnemployedUnits.Remove(unit);

        EmployedUnits[employer][index] = unit;
        employer.NewEmployee(unit);
        unit.OnEmploy(employer);

        //Debug.Log($"Employed {unit.name}. {employer} now has {employer.CurrentEmployeeCount()} employees!");
    }

    public void Fire(CivilianBuilding employer, EmployableUnit unit)
    {
        if (!EmployerExists(employer)) return;
        if (!IsEmployed(unit)) return;

        for (int i = 0; i < EmployedUnits[employer].Length; i++)
        {
            if (EmployedUnits[employer][i] == unit)
            {
                EmployedUnits[employer][i] = null;
                break;
            }
        }
        UnemployedUnits.Add(unit);
        unit.OnFire();
        //Debug.Log($"Fired {unit.name}. {employer} now has {employer.CurrentEmployeeCount()} employees!");
    }

    public bool IsEmployed(EmployableUnit unit)
    {
        return UnemployedUnits.Contains(unit) == false;
    }

    public bool EmployerExists(CivilianBuilding employer)
    {
        return EmployedUnits.ContainsKey(employer);
    }

    public bool HasVacancy(CivilianBuilding employer)
    {
        if (!EmployerExists(employer)) return false;

        foreach (var employee in EmployedUnits[employer])
        {
            if (employee == null) return true;
        }
        return false;
    }

    public int FindVacancyIndex(CivilianBuilding employer)
    {
        if (!EmployerExists(employer)) return -1;

        for (int i = 0; i < EmployedUnits[employer].Length; i++)
        {
            if (EmployedUnits[employer][i] == null) return i;
        }
        return -1;
    }
}

using UnityEngine;

public class MilitaryBuilding : AssignableBuilding
{
    public Soldier[] Soldiers { get; set; }


    private new void Start()
    {
        base.Start();

        Soldiers = new Soldier[base.MaxEmployees];
        Overseer.Instance.GetManager<EmploymentManager>().NewMilitaryEmployer(this);

        //Building.BuildingStartUp(); //fix this
    }

    public void NewSoldier(Soldier soldier)
    {
        for (int i = 0; i < Soldiers.Length; i++)
        {
            if (Soldiers[i] == null)
            {
                Soldiers[i] = soldier;
                return;
            }
        }
    }

    public int CurrentSoldierCount()
    {
        int count = 0;
        foreach (var soldier in Soldiers)
        {
            if (soldier != null) count++;
        }
        return count;
    }


    public virtual void Garrison(Soldier soldier)
    {
    }
}

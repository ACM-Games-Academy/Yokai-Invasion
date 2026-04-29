using UnityEngine;

public class SpawnVillagerButton : MonoBehaviour
{
    public void Spawn()
    {
            Overseer.Instance.GetManager<UnitSpawner>().
                SpawnByIndex(Overseer.Instance.GetManager<UnitSpawner>().IndexDictionary["Farmer"]); 
    }
}

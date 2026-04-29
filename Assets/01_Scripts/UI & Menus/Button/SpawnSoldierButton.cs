using UnityEngine;

public class SpawnSoldierButton : MonoBehaviour
{
    public void Spawn()
    {
        Overseer.Instance.GetManager<UnitSpawner>().
                    SpawnByIndex(Overseer.Instance.GetManager<UnitSpawner>().IndexDictionary["Ashigaru"]);
    }

}

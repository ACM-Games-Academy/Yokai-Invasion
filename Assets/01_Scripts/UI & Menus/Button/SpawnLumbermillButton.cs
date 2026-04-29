using UnityEngine;

public class SpawnLumbermillButton : MonoBehaviour, SpawnButton
{
    public void Spawn()
    {
        if (Overseer.Instance.GetManager<BuildingSpawner>().BuildModeState == BuildingSpawner.BuildMode.active)
        {
            Overseer.Instance.GetManager<BuildingSpawner>().
                SpawnByIndex(Overseer.Instance.GetManager<BuildingSpawner>().IndexDictionary["Lumbermill"]);
        }
    }
}

using UnityEngine;

public class GoldDebugButton : MonoBehaviour
{
    public void Press()
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(10);
    }
}

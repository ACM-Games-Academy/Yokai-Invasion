using UnityEngine;

public class WoodDebugButton : MonoBehaviour
{
    public void Press()
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseWood(10);
    }
}

using UnityEngine;

public class FoodDebugMenu : MonoBehaviour
{
    public void Press()
    {
        Overseer.Instance.GetManager<ResourceManager>().IncreaseFood(10);
    }
}

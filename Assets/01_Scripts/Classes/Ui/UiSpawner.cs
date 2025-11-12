using UnityEngine;

public class UiSpawner : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(Overseer.Instance.Settings.UiCanvas);
    }
}

using UnityEngine;

public class UiSpawner : MonoBehaviour
{
    public GameObject uiCanvas;
    private void Awake()
    {
        uiCanvas = Instantiate(Overseer.Instance.Settings.UiCanvas);
    }
}

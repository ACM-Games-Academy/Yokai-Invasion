using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMenuInput : MonoBehaviour
{
    private static GameObject debugMenu;

    private void Start()
    {
        debugMenu = GameObject.Find("Debug Menu");
        debugMenu.SetActive(false);
    }
    public static void ToggleDebugMenu(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        debugMenu.SetActive(!debugMenu.activeSelf);
        Debug.Log("debug menu active state change");
    }
}

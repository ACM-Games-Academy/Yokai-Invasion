using UnityEngine;
using UnityEngine.InputSystem;

public class HiringMenuInput : MonoBehaviour
{
    private static GameObject hiringMenu;

    private void Start()
    {
        hiringMenu = GameObject.Find("Hiring Menu");
        hiringMenu.SetActive(false);
    }
    public static void ToggleHiringMenu(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        hiringMenu.SetActive(!hiringMenu.activeSelf);
    }
}

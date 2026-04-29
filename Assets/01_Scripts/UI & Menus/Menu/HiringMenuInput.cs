using UnityEngine;
using UnityEngine.InputSystem;

public class HiringMenuInput : MonoBehaviour
{
    private static GameObject hiringMenu;

    private static AudioSettings audioSettings;

    private void Start()
    {
        hiringMenu = GameObject.Find("Hiring Menu");
        hiringMenu.SetActive(false);

        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }
    public static void ToggleHiringMenu(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        hiringMenu.SetActive(!hiringMenu.activeSelf);

        //  [14] Play_Scroll_Open - Plays scroll opening/closing sound
        audioSettings.Events[14].Post(hiringMenu);
    }
}

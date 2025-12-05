using UnityEngine;
using UnityEngine.InputSystem;

public class CommandManager : MonoBehaviour
{
    private static AudioSettings audioSettings;

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }

    public static void HandleMoveInput(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitInfo))
        {
            var destination = hitInfo.point;

            foreach (var unit in Overseer.Instance.GetManager<SelectionManager>().SelectedUnits)
            {
                unit.SetDestination(destination);

                //  [28] Play_Command_Ashigaru - Plays voice for responding to commands
                audioSettings.Events[28].Post(unit.gameObject);
            }
        }
    }
}

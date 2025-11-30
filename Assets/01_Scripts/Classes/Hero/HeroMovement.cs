using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    private static Vector2 movementInput = Vector2.zero;
    private static int rotationDegree = 135;

    [SerializeField] private HeroSettings heroSettings;

    private void Update()
    {
        Vector3 newMovementInput = new Vector3(movementInput.x, 0, movementInput.y);
        transform.position += newMovementInput * Time.deltaTime * heroSettings.MovementSpeed;

    }

    public static void SyncMovementInput(InputAction.CallbackContext input) 
    { 
        movementInput = Quaternion.Euler(0, 0, rotationDegree) * input.ReadValue<Vector2>();
    }
}

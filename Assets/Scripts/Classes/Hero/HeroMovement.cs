using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    private Vector2 movementInput = Vector2.zero;
    private int rotationDegree = 135;

    [SerializeField] private HeroSettings heroSettings;

    private void Update()
    {
        Vector3 newMovementInput = new Vector3(movementInput.x, 0, movementInput.y);
        transform.position += newMovementInput * Time.deltaTime * heroSettings.MovementSpeed;
    }

    public void SyncMovementInput(InputAction.CallbackContext input) 
    { 
        movementInput = input.ReadValue<Vector2>();

        movementInput = Quaternion.Euler(0, 0, rotationDegree) * movementInput;

    }
}

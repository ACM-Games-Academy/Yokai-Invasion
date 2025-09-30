using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Vector2 movementInput = Vector2.zero;
    [SerializeField] float movement;

    private void Update()
    {
        Vector3 newMovementInput = new Vector3(movementInput.x, 0, movementInput.y);
        transform.position += newMovementInput * Time.deltaTime * movement;
    }

    public void SyncMovementInput(InputAction.CallbackContext input) 
    { 
        movementInput = input.ReadValue<Vector2>();
    }
}

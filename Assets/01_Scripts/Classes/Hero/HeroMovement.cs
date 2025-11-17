using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    private static Vector2 movementInput = Vector2.zero;
    private static int rotationDegree = 135;

    [SerializeField] private HeroSettings heroSettings;

    [SerializeField] private float groundDistanceCheck;

    [SerializeField] Vector3 velocity;


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private bool IsGrounded()
    {
        RaycastHit hitInfo;
        MeshCollider col = GetComponentInChildren<MeshCollider>();
        Vector3 rayCastOffset = -Vector3.up * col.bounds.extents.y;
        Physics.Raycast(transform.position + rayCastOffset, -transform.up, out hitInfo, groundDistanceCheck);
        Debug.DrawRay(transform.position + rayCastOffset, -transform.up * groundDistanceCheck, Color.red, 0.1f);
        
         if (hitInfo.collider != null)
        {
            if (hitInfo.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        Vector3 newMovementInput = new Vector3(movementInput.x, 0, movementInput.y);
        transform.position += newMovementInput * Time.deltaTime * heroSettings.MovementSpeed;
        float forwardsMovement = newMovementInput.z;
        float sidewaysMovement = newMovementInput.x;

        Vector3 horizontalVel = (transform.forward * forwardsMovement * heroSettings.MovementSpeed) + (transform.right * sidewaysMovement * heroSettings.MovementSpeed);
        float verticalVel = rb.linearVelocity.y;

        if (IsGrounded())
        {
            if (verticalVel < 0f)
            {
                verticalVel = 0;
            }

            velocity = rb.linearVelocity;

        }
    }

    public static void SyncMovementInput(InputAction.CallbackContext input)
    {
        movementInput = Quaternion.Euler(0, 0, rotationDegree) * input.ReadValue<Vector2>();
    }
}

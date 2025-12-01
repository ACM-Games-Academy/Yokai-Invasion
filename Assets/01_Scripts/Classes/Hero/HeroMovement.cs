using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    private static Vector2 movementInput = Vector2.zero;
    private static int rotationDegree = 135;
    private static Vector3 rotationInput;
    private float currentVelocity;

    [SerializeField] private HeroSettings heroSettings;
    [SerializeField] private float smoothTime;

    private GameObject hero;
    private CallDeath callDeath;
    private bool gameOver;
    private void Start()
    {
        hero = GameObject.Find("Coloured Hero");
        callDeath = hero.GetComponent<CallDeath>();
        callDeath.HeroDead += GameOver;
    }

    private void Update()
    {
        if (!gameOver)
        {
            Move();
            Rotate();
        }
    }
    private void Move()
    {
        Vector3 newMovementInput = new Vector3(movementInput.x, 0, movementInput.y);
        transform.position += newMovementInput * Time.deltaTime * heroSettings.MovementSpeed;
    }

    private void Rotate()
    {
        rotationInput = new Vector3(movementInput.x, 0, movementInput.y);
        var rotationAngle = Mathf.Atan2(rotationInput.x, rotationInput.z) * Mathf.Rad2Deg;
        var smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
    }
    public static void SyncMovementInput(InputAction.CallbackContext input) 
    { 
        movementInput = Quaternion.Euler(0, 0, rotationDegree) * input.ReadValue<Vector2>();
    }

    private void GameOver()
    {
        gameOver = true;
    }
}

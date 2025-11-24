using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset;
    private Vector3 newPosition;

    [SerializeField]
    private float edgeSize = 100f;
    [SerializeField]
    private float freeMoveSpeed = 0.5f; 
    [SerializeField]
    private float lockedMoveSpeed = 0.5f;

    private Vector2 mousePos;

    private enum CameraState : byte
    {
        followHero,
        freeMove
    }
    private CameraState currentState = CameraState.followHero;

    private static bool isFollowingPlayer = true;

    private void Update()
    {
        if (isFollowingPlayer)
        {
            currentState = CameraState.followHero;
            isFollowingPlayer = false;
        }

        mousePos = Mouse.current.position.ReadValue();

        EnterFreeMoveCheck();

        switch (currentState)
        {
            case CameraState.followHero:
                FollowHero();
                break;
            case CameraState.freeMove:
                FreeMove();
                break;
        }

        newPosition = new Vector3(newPosition.x, 0, newPosition.z);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition + offset, Time.deltaTime * lockedMoveSpeed);
    }

    private void FollowHero()
    {
        newPosition = transform.position;
    }

    private void FreeMove()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Flatten to XZ plane (ignore camera pitch)
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = Vector3.zero;

        if (mousePos.x > Screen.width - edgeSize)
            moveDir += right;
        else if (mousePos.x < edgeSize)
            moveDir -= right;

        if (mousePos.y > Screen.height - edgeSize)
            moveDir += forward;
        else if (mousePos.y < edgeSize)
            moveDir -= forward;

        newPosition += moveDir * freeMoveSpeed * Time.deltaTime;
    }

    private void EnterFreeMoveCheck()
    {
        if (mousePos.x > Screen.width - edgeSize ||
            mousePos.x < edgeSize ||
            mousePos.y > Screen.height - edgeSize ||
            mousePos.y < edgeSize)
        {
            currentState = CameraState.freeMove;
        }
    }

    public static void ToggleFollowPlayer(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        isFollowingPlayer = true;
    }
}
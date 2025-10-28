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

    private Vector2 mousePos;

    private int rotationDegree = 135;

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

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition + offset, Time.deltaTime);
    }

    private void FollowHero()
    {
        newPosition = transform.position;
    }

    private void FreeMove()
    {
        MoveToEdge(mousePos.x, Screen.width, -transform.right);
        MoveToEdge(mousePos.y, Screen.height, transform.up);
    }

    private void MoveToEdge(float axis, int screenDimension, Vector3 direction)
    {
        if (axis > screenDimension - edgeSize)
        {
            newPosition += (direction * freeMoveSpeed * Time.deltaTime);
        }
        else if (axis < edgeSize)
        {
            newPosition += (direction * -freeMoveSpeed * Time.deltaTime);
        }
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
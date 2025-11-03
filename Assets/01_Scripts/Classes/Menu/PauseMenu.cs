using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuCanvas;

    // Drag your camera/player controller script here in the inspector
    public MonoBehaviour cameraController;

    public enum PauseState
    {
        UNPAUSED,
        PAUSED,
        BECOMING_PAUSED,
        BECOMING_UNPAUSED,
    }

    public static PauseState currentPauseState;

    private void Update()
    {
        switch (currentPauseState)
        {
            case PauseState.BECOMING_PAUSED:
                Pause();
                break;
            case PauseState.BECOMING_UNPAUSED:
                Resume();
                break;
        }
    }

    public void Pause()
    {
        Debug.Log("Game Paused");
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        cameraController.enabled = false;

        currentPauseState = PauseState.PAUSED;
    }

    public void Resume()
    {
        Debug.Log("Game Resumed");
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        cameraController.enabled = true;

        currentPauseState = PauseState.UNPAUSED;
    }


    public static void TogglePause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        switch (currentPauseState)
        {
            case PauseState.UNPAUSED:
                currentPauseState = PauseState.BECOMING_PAUSED;
                break;
            case PauseState.PAUSED:
                currentPauseState = PauseState.BECOMING_UNPAUSED;
                break;
            default:
                // Do nothing if we're already in the process of pausing/unpausing
                return;
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}

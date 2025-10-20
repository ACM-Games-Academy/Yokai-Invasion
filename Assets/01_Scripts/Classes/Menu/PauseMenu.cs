using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject PauseMenuCanvas;

    // Drag your camera/player controller script here in the inspector
    public MonoBehaviour cameraController;

    void Start()
    {
        // Time.timeScale = 1f;
        PauseMenuCanvas.SetActive(false);
    }

    // Use input manager
    void Update()
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        PauseMenuCanvas.SetActive(true);
        // Time.timeScale = 0f;
        paused = true;

        // Stop camera/player movement
        if (cameraController != null)
            cameraController.enabled = false;
    }

    public void Resume()
    {
        PauseMenuCanvas.SetActive(false);
        // Time.timeScale = 1f;
        paused = false;

        // Resume camera/player movement
        if (cameraController != null)
            cameraController.enabled = true;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void TogglePause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        paused = !paused;
    }

    public void QuitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}

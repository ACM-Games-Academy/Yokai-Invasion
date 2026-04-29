using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuCanvas;

    private AudioSettings audioSettings;

    public enum PauseState
    {
        UNPAUSED,
        PAUSED,
        BECOMING_PAUSED,
        BECOMING_UNPAUSED,
    }

    public static PauseState CurrentPauseState;

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }

    private void Update()
    {
        switch (CurrentPauseState)
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
        Time.timeScale = 0f;

        //  [14] Play_Scroll_Open - Plays scroll opening/closing sound
        audioSettings.Events[14].Post(PauseMenuCanvas);
        //  [22] Pause_Music - Fades out + pauses music
        audioSettings.Events[22].Post(PauseMenuCanvas);

        CurrentPauseState = PauseState.PAUSED;
    }

    public void Resume()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1f;
        
        //  [14] Play_Scroll_Open - Plays scroll opening/closing sound
        audioSettings.Events[14].Post(PauseMenuCanvas);
        //  [23] Resume_Music - Resumes + fades in music
        audioSettings.Events[23].Post(PauseMenuCanvas);

        CurrentPauseState = PauseState.UNPAUSED;
    }


    public static void TogglePause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        switch (CurrentPauseState)
        {
            case PauseState.UNPAUSED:
                CurrentPauseState = PauseState.BECOMING_PAUSED;
                break;
            case PauseState.PAUSED:
                CurrentPauseState = PauseState.BECOMING_UNPAUSED;
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

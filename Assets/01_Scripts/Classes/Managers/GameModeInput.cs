using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class GameModeInput : MonoBehaviour
{
    private AudioSettings audioSettings;

    private static GameObject pauseMenu;
    private GameObject gameOverMenu;
    private GameObject tutorialScreen;
    private GameObject hero;
    private CallDeath callDeath;

    
    private static PauseMenu pauseMenuScript;

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;

        tutorialScreen = GameObject.Find("Tutorial Screen");
        tutorialScreen.SetActive(false);

        pauseMenuScript = gameObject.GetComponent<PauseMenu>();

        pauseMenu = GameObject.Find("Pause Menu");
        pauseMenu.SetActive(false);

        gameOverMenu = GameObject.Find("Game Over Menu");
        gameOverMenu.SetActive(false);

        hero = GameObject.Find("Coloured Hero");
        callDeath = hero.GetComponent<CallDeath>();
        callDeath.HeroDead += GameOver;
    }
    public static void TogglePauseMenu(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (!pauseMenu.activeSelf)
        {
            pauseMenuScript.Resume();
        }
        else
        {
            pauseMenuScript.Pause();
        }
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        pauseMenuScript.Resume();   
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);

        //  [14] Play_Scroll_Open - Plays scroll opening/closing sound
        audioSettings.Events[14].Post(gameOverMenu);
    }

    public void RestartScene()
    {
        //  [14] Play_Scroll_Open - Plays scroll opening/closing sound
        audioSettings.Events[14].Post(gameOverMenu);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TutorialScreen()
    {
        tutorialScreen.SetActive(!tutorialScreen.activeSelf);

        //  [14] Play_Scroll_Open - Plays scroll opening/closing sound
        audioSettings.Events[14].Post(tutorialScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}

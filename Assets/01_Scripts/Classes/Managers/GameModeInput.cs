using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        pauseMenuScript.Pause();
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TutorialScreen()
    {
        tutorialScreen.SetActive(!tutorialScreen.activeSelf);
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

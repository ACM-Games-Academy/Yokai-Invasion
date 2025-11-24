using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeInput : MonoBehaviour
{
    private static GameObject pauseMenu;
    private GameObject gameOverMenu;
    private GameObject hero;
    private HeroAttack heroAttack;

    private void Start()
    {
        pauseMenu = GameObject.Find("Pause Menu");
        pauseMenu.SetActive(false);
        gameOverMenu = GameObject.Find("Game Over Menu");
        gameOverMenu.SetActive(false);

        hero = GameObject.Find("TempHero");
        heroAttack = hero.GetComponent<HeroAttack>();
        heroAttack.HeroDead += GameOver;
    }
    public static void TogglePauseMenu(InputAction.CallbackContext input)
    {
        if (!input.started) return;

        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}

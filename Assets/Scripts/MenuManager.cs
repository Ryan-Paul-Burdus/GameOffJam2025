using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameObject GameplayMenuHolder;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject GameOverMenu;

    public TextMeshProUGUI ScoreText;

    public bool IsPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (IsPaused)
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        IsPaused = true;

        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        GameOverMenu.SetActive(false);

        GameplayMenuHolder.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        GameplayMenuHolder.SetActive(false);

        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        GameOverMenu.SetActive(false);

        IsPaused = false;
    }

    public void OpenOptionsMenu()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        GameOverMenu.SetActive(false);
    }

    public void OpenGameOverMenu(string scoreText)
    {
        IsPaused = true;

        ScoreText.text = scoreText;

        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        GameOverMenu.SetActive(true);

        GameplayMenuHolder.SetActive(true);
    }

    public void OpenMainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGameplayMenuButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager Instance { get; private set; }

    public GameObject MenuObject;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject GameOverScreen;

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

    #region Pause

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }

    private void OpenPauseMenu()
    {
        IsPaused = true;
        Time.timeScale = 0.0f;

        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        GameOverScreen.SetActive(false);

        MenuObject.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        MenuObject.SetActive(false);

        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        GameOverScreen.SetActive(false);

        IsPaused = false;
        Time.timeScale = 1.0f;
    }

    #endregion Pause

    public void OpenOptionsMenu()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        GameOverScreen.SetActive(false);
    }

    public void OpenGameOverScreen(string scoreText)
    {
        IsPaused = true;
        Time.timeScale = 0.0f;

        ScoreText.text = scoreText;

        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        GameOverScreen.SetActive(true);

        MenuObject.SetActive(true);
    }

    public void LoadMainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}

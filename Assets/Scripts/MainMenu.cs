using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGameplayMenuButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}

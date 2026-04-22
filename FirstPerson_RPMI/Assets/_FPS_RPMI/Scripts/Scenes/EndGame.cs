using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [Header("Scenes")]
    public string mainMenuScene = "MainMenu";

    // 🔄 RESTART
    public void RestartLevel()
    {
        Time.timeScale = 1f; // 🔥 importante (por si pausaste el juego)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏠 MAIN MENU
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}
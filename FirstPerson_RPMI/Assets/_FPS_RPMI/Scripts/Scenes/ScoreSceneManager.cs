using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI lastScoreText;

    void Start()
    {
        // 🔥 Leer datos guardados
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        // 🔥 Mostrar en pantalla (solo números)
        bestScoreText.text = bestScore.ToString();
        lastScoreText.text = lastScore.ToString();
    }

    // 🔙 BOTÓN MENU
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SCN_Main Menu");
    }

    // 🔁 BOTÓN RESTART (opcional si lo usas aquí)
    public void RestartGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
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
        Time.timeScale = 1f;

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        bestScoreText.text = bestScore.ToString();
        lastScoreText.text = lastScore.ToString();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SCN_Main Menu");
    }
}
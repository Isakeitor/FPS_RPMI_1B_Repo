using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        instance = this;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void EndGame()
    {
        // 🔥 GUARDAR SCORE ACTUAL
        PlayerPrefs.SetInt("LastScore", score);

        // 🔥 COMPROBAR HIGH SCORE
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        // 🔥 IR A ESCENA DE GAME OVER / SCORE
        SceneManager.LoadScene("ScoreScene");
    }

    void Start()
    {
        scoreText.text = "Score: 0";
    }
}
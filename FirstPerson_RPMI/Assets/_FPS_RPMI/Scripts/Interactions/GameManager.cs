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
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("GameOver");
    }

    void Start()
    {
        scoreText.text = "Score: 0";
    }
}
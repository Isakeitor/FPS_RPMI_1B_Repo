using UnityEngine;
using TMPro;

public class EndGameScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void OnEnable()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        if (GameManager.instance != null)
        {
            scoreText.text = "Score: " + GameManager.instance.score;
        }
        else
        {
            scoreText.text = "Score: 0";
        }
    }
}

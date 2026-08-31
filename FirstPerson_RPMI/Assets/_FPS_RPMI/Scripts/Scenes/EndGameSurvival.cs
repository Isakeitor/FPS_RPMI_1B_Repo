using UnityEngine;
using TMPro;

public class EndGameSurvival : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        Timer timer = FindFirstObjectByType<Timer>();

        if (timer != null)
        {
            float time = timer.GetElapsedTime();

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);

            scoreText.text = string.Format(
                "Score: {0:00}:{1:00}",
                minutes,
                seconds
            );
        }
        else
        {
            scoreText.text = "Score: 00:00";
        }
    }
}
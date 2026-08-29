using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool timerRunning = true;

    private void Update()
    {
        if (!timerRunning)
            return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;
    }
}
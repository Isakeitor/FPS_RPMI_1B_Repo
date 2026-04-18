using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject infoPanel;

    [Header("Scenes")]
    public string easyScene = "EasyScene";
    public string hardScene = "HardScene";
    public string bestScoreScene = "ScoreScene";

    [Header("Audio")]
    public Image volumeImage;
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    bool isMuted = false;

    void Start()
    {
        ShowMainMenu();
    }

    // 🎮 PLAY
    public void PlayEasy() => SceneManager.LoadScene(easyScene);
    public void PlayHard() => SceneManager.LoadScene(hardScene);
    public void OpenBestScore() => SceneManager.LoadScene(bestScoreScene);

    // ⚙️ OPTIONS
    public void OpenOptions()
    {
        HideAllPanels();
        optionsPanel.SetActive(true);
    }

    // 📖 CONTROLS
    public void OpenControls()
    {
        HideAllPanels();
        controlsPanel.SetActive(true);
    }

    // 📦 INFO
    public void OpenInfo()
    {
        HideAllPanels();
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // 🔙 BACK SYSTEM (INTELIGENTE)
    public void BackToMenu()
    {
        if (controlsPanel.activeSelf || infoPanel.activeSelf)
        {
            HideAllPanels();
            optionsPanel.SetActive(true);
        }
        else if (optionsPanel.activeSelf)
        {
            HideAllPanels();
            mainPanel.SetActive(true);
        }
    }

    // ❌ EXIT
    public void ExitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }

    // 🔊 VOLUME TOGGLE
    public void ToggleVolume()
    {
        isMuted = !isMuted;

        AudioListener.volume = isMuted ? 0f : 1f;

        if (volumeImage != null)
        {
            volumeImage.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
        }
    }

    // 🧠 HELPERS
    void HideAllPanels()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        infoPanel.SetActive(false);
    }

    void ShowMainMenu()
    {
        HideAllPanels();
        mainPanel.SetActive(true);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject controlsPanel;

    [Header("Player")]
    public FPController playerController;

    [Header("Scenes")]
    public string mainMenuScene = "MainMenu";
    public string currentSceneName;

    [Header("Audio")]
    public AudioListener audioListener;

    [Header("UI Volume")]
    public Image volumeImage;
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    bool isPaused = false;
    bool isMuted = false;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (controlsPanel.activeSelf)
        {
            CloseControls();
            return;
        }

        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;

        pausePanel.SetActive(true);
        controlsPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.canMove = false;
        playerController.canLook = false;
    }

    public void Resume()
    {
        isPaused = false;

        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.canMove = true;
        playerController.canLook = true;
    }

    // 🎮 CONTINUAR
    public void OnResumeButton()
    {
        Resume();
    }

    // 🚪 QUIT AL MENÚ
    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    // 🔁 RESTART NIVEL
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentSceneName);
    }

    // 🔊 TOGGLE VOLUMEN
    public void OnVolumeButton()
    {
        isMuted = !isMuted;

        AudioListener.volume = isMuted ? 0f : 1f;

        if (volumeImage != null)
        {
            volumeImage.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
        }
    }

    // 📖 CONTROLES
    public void OnControlsButton()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
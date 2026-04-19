using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CajaFuerte : Interactable
{
    public FPController playerController;
    public string victorySceneName = "Victory";
    public string correctCode = "1234";

    public GameObject codePanel;
    public InputField codeInput;

    void Start()
    {
        interactionText = "Open Safe";
    }

    public override void Interact()
    {
        base.Interact();

        Time.timeScale = 0f;
        codePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.canMove = false;
        playerController.canLook = false;
    }

    public void CheckCode()
    {
        if (codeInput.text == correctCode)
        {
            Time.timeScale = 1f;
            playerController.canMove = true;
            playerController.canLook = true;
            SceneManager.LoadScene(victorySceneName);
        }
        else
        {
            codeInput.text = "";
        }
    }

    public void ClosePanel()
    {
        codePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.canMove = true;
        playerController.canLook = true;
    }

    void Update()
    {
        if (codePanel.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ClosePanel();
        }
    }
}
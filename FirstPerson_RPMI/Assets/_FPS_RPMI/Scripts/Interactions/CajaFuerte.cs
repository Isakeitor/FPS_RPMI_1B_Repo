using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CajaFuerte : Interactable
{
    public FPController playerController;
    public string correctCode = "1234";

    [Header("UI")]
    public GameObject codePanel;
    public InputField codeInput;
    public GameObject victoryPanel;

    [Header("Audio")]
    public AudioSource correctCodeAudio;

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
            if (correctCodeAudio != null)
            {
                correctCodeAudio.Play();
            }

            codePanel.SetActive(false);

            victoryPanel.SetActive(true);

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerController.canMove = false;
            playerController.canLook = false;
        }
        else
        {
            codeInput.text = "";
        }
    }

    public void ClosePanel()
    {
        codePanel.SetActive(false);

        Time.timeScale = 1f;

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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CajaFuerte : Interactable
{
    public FPController playerController;
    public string victorySceneName = "Victory";
    public string correctCode = "1234";

    [Header("UI")]
    public GameObject codePanel;
    public InputField codeInput;

    public override void Interact()
    {
        Time.timeScale = 0f; // pausa el juego
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
            Debug.Log("Código incorrecto");
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
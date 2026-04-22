using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class WallCode : Interactable
{
    public FPController playerController;
    public string correctCode = "1234";

    [Header("UI")]
    public GameObject codePanel;
    public InputField codeInput;

    [Header("Animation")]
    public Animator targetAnimator;
    public string animationTrigger = "Open";

    bool activated = false;

    void Start()
    {
        interactionText = "Enter Code";
    }

    public override void Interact()
    {
        base.Interact();

        if (activated) return;

        OpenPanel();
    }

    void OpenPanel()
    {
        Time.timeScale = 0f;
        codePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.canMove = false;
        playerController.canLook = false;

        EventSystem.current.SetSelectedGameObject(codeInput.gameObject);
        codeInput.ActivateInputField();
    }
    public void CheckCode()
    {
        if (codeInput.text == correctCode)
        {
            ActivateWall();
        }
        else
        {
            codeInput.text = "";
        }
    }

    void ActivateWall()
    {
        activated = true;

        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger(animationTrigger);
        }

        ClosePanel();
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
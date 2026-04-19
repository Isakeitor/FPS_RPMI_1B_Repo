using UnityEngine;

public class Drawer : Interactable
{
    public Animator anim;
    bool isOpen = false;

    void Start()
    {
        interactionText = "Open";
    }

    public override void Interact()
    {
        base.Interact();

        isOpen = !isOpen;
        anim.SetBool("Open", isOpen);

        SetInteractionText(isOpen ? "Close" : "Open");
    }
}
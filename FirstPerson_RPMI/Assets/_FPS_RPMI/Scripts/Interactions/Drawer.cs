using UnityEngine;

public class Drawer : Interactable
{
    public Animator anim;

    bool isOpen = false;

    public override void Interact()
    {
        base.Interact();

        isOpen = !isOpen;

        if (isOpen)
            anim.SetTrigger("Open");
        else
            anim.SetTrigger("Close");
    }
}
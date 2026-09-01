using UnityEngine;

public class Drawer : Interactable
{
    public Animator anim;

    [Header("Audio")]
    public AudioSource audioSource;

    bool isOpen = false;

    public override void Interact()
    {
        base.Interact();

        isOpen = !isOpen;

        if (isOpen)
            anim.SetTrigger("Open");
        else
            anim.SetTrigger("Close");

        if (audioSource != null)
            audioSource.Play();
    }
}
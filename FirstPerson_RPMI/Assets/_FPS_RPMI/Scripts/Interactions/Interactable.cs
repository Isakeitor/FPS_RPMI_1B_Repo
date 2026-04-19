using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    [TextArea]
    public string interactionText = "Interact";
    public bool canInteract = true;

    [Header("Outline")]
    public GameObject outlineObject; // objeto con outline (hijo)

    [Header("Audio")]
    public AudioClip interactSound;
    AudioSource audioSource;

    bool isFocused = false;

    void Start()
    {
        if (outlineObject != null)
            outlineObject.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public virtual void Interact()
    {
        if (!canInteract) return;

        if (interactSound != null)
            audioSource.PlayOneShot(interactSound);
    }

    public virtual void OnFocus()
    {
        if (isFocused) return;
        isFocused = true;

        if (outlineObject != null)
            outlineObject.SetActive(true);
    }

    public virtual void OnLoseFocus()
    {
        if (!isFocused) return;
        isFocused = false;

        if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    public void SetInteractionText(string newText)
    {
        interactionText = newText;
    }
}
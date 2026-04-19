using UnityEngine;

public class FBXInteractable : Interactable
{
    public GameObject targetObject;
    bool isActive = true;

    void Start()
    {
        interactionText = "Turn Off";
    }

    public override void Interact()
    {
        base.Interact();

        isActive = !isActive;
        targetObject.SetActive(isActive);

        SetInteractionText(isActive ? "Turn Off" : "Turn On");
    }
}
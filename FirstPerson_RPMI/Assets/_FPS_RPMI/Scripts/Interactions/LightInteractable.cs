using UnityEngine;

public class LightInteractable : Interactable
{
    public Light targetLight;
    bool isOn = true;

    void Start()
    {
        interactionText = "Turn Off Light";
    }

    public override void Interact()
    {
        base.Interact();

        isOn = !isOn;
        targetLight.enabled = isOn;

        SetInteractionText(isOn ? "Turn Off Light" : "Turn On Light");
    }
}
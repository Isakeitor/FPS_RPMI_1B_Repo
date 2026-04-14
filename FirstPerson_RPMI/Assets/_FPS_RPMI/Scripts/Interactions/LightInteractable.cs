using UnityEngine;

public class LightInteractable : Interactable
{
    public Light targetLight;

    public override void Interact()
    {
        if (targetLight != null)
            targetLight.enabled = !targetLight.enabled;
    }
}
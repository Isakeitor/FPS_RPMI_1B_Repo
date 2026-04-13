using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactLayer;
    public Camera cam;

    public GameObject interactUI;

    private Interactable currentInteractable;

    void Update()
    {
        DetectInteractable();
    }

    void DetectInteractable()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactLayer))
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();

            if (currentInteractable != null)
            {
                interactUI.SetActive(true);
                return;
            }
        }

        currentInteractable = null;
        interactUI.SetActive(false);
    }

    // 👉 LLAMADO DESDE INPUT SYSTEM
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentInteractable != null)
                currentInteractable.Interact();
        }
    }

    public bool HasInteractable()
    {
        return currentInteractable != null;
    }

    public void Interact()
    {
        if (currentInteractable != null)
            currentInteractable.Interact();
    }
}
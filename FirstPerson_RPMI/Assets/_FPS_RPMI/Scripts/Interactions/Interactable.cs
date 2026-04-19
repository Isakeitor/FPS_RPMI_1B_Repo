using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    [TextArea]
    public string interactionText = "Interact";

    [Header("Settings")]
    public bool canInteract = true;

    // 👉 Método principal que todos los hijos sobrescriben
    public virtual void Interact()
    {
        if (!canInteract) return;

        Debug.Log("Interactuado con: " + gameObject.name);
    }

    // 👉 Opcional: cuando el jugador mira el objeto
    public virtual void OnFocus()
    {
        // Aquí puedes añadir highlight, outline, etc.
    }

    // 👉 Opcional: cuando deja de mirar el objeto
    public virtual void OnLoseFocus()
    {
        // Quitar highlight, etc.
    }

    // 👉 Cambiar texto dinámicamente (útil para puertas, luces, etc.)
    public void SetInteractionText(string newText)
    {
        interactionText = newText;
    }
}
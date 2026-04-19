using UnityEngine;

public class DragObject : Interactable
{
    public float dragSpeed = 10f;
    bool isDragging = false;

    Transform player;

    void Start()
    {
        interactionText = "Drag";
        player = Camera.main.transform;
    }

    public override void Interact()
    {
        base.Interact();

        isDragging = !isDragging;
        SetInteractionText(isDragging ? "Release" : "Drag");
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 target = player.position + player.forward * 2f;
            target.y = transform.position.y;

            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * dragSpeed);
        }
    }
}
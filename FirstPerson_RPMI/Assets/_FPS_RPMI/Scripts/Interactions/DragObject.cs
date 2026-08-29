using UnityEngine;

public class DragObject : Interactable
{
    [Header("Dragging")]
    [SerializeField] private float dragSpeed = 10f;
    [SerializeField] private float dragDistance = 2f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Break System")]
    [SerializeField] private int hitsToBreak = 3;

    private bool isDragging = false;
    private int currentHits = 0;

    private Transform player;

    private void Start()
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

    private void Update()
    {
        if (!isDragging || player == null)
            return;

        // Mantener el objeto delante del jugador
        Vector3 targetPosition =
            player.position + player.forward * dragDistance;

        targetPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * dragSpeed
        );

        // Girar el objeto con el jugador
        Quaternion targetRotation = Quaternion.Euler(
            0f,
            player.eulerAngles.y,
            0f
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    public void TakeBulletHit()
    {
        currentHits++;

        if (currentHits >= hitsToBreak)
        {
            BreakObject();
        }
    }

    private void BreakObject()
    {
        isDragging = false;

        gameObject.SetActive(false);
    }
}
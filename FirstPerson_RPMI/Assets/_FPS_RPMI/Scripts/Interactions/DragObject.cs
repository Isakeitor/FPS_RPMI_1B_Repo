using UnityEngine;

public class DragObject : Interactable
{
    [Header("Dragging")]
    [SerializeField] private float dragSpeed = 10f;
    [SerializeField] private float dragDistance = 2f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundOffset = 0.05f;

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

        // Posición delante del jugador, incluyendo altura
        Vector3 targetPosition =
            player.position + player.forward * dragDistance;

        // Detectar el suelo debajo del objeto
        Ray ray = new Ray(
            targetPosition + Vector3.up * 5f,
            Vector3.down
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            10f,
            groundLayer
        ))
        {
            // Mantener el objeto por encima del suelo
            float objectHeight = GetComponent<Collider>().bounds.extents.y;

            targetPosition.y =
                hit.point.y + objectHeight + groundOffset;
        }

        // Movimiento
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * dragSpeed
        );

        // Rotación con el jugador
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
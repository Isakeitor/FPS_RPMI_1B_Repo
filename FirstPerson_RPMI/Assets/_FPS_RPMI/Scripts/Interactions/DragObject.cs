using UnityEngine;

public class DragObject : Interactable
{
    [Header("Dragging")]
    [SerializeField] private float dragSpeed = 10f;

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
        if (isDragging)
        {
            Vector3 target = player.position + player.forward * 2f;
            target.y = transform.position.y;

            transform.position = Vector3.Lerp(
                transform.position,
                target,
                Time.deltaTime * dragSpeed
            );
        }
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
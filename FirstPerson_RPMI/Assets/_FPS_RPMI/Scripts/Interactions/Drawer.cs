using UnityEngine;

public class Drawer : Interactable
{
    public Transform drawer; // el cajón
    public Vector3 openOffset; // cuánto se mueve (ej: (0, 0, 0.5f))
    public float speed = 5f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        closedPos = drawer.localPosition;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 target = isOpen ? openPos : closedPos;

            drawer.localPosition = Vector3.Lerp(drawer.localPosition, target, Time.deltaTime * speed);

            if (Vector3.Distance(drawer.localPosition, target) < 0.01f)
            {
                drawer.localPosition = target;
                isMoving = false;
            }
        }
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        isMoving = true;
    }
}
using UnityEngine;

public class Diamond : Interactable
{
    public int points = 10;
    public GameObject particles;

    void Start()
    {
        interactionText = "Collect";
    }

    public override void Interact()
    {
        base.Interact();

        GameManager.instance.AddPoints(points);

        if (particles != null)
            Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
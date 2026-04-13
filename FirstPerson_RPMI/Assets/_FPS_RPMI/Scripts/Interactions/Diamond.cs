using UnityEngine;

public class Diamond : Interactable
{
    public int points = 10;
    public GameObject particles;

    public override void Interact()
    {
        GameManager.instance.AddPoints(points);

        if (particles != null)
            Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
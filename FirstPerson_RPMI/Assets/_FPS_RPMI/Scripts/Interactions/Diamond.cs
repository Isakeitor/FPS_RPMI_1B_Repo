using UnityEngine;

public class Diamond : Interactable
{
    public int points = 10;
    public GameObject particles;

    [Header("Audio")]
    [SerializeField] private AudioSource pickupAudio;

    void Start()
    {
        interactionText = "Collect";
    }

    public override void Interact()
    {
        base.Interact();

        GameManager.instance.AddPoints(points);

        // Sonido al recoger
        if (pickupAudio != null)
            pickupAudio.Play();

        if (particles != null)
            Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
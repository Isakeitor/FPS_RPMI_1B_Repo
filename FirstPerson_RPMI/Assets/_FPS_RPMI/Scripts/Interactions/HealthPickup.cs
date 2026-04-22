using UnityEngine;

public class HealthPickup : Interactable
{
    public GameObject particles;

    void Start()
    {
        interactionText = "Heal";
    }

    public override void Interact()
    {
        base.Interact();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.HealFull();
            }
            else
            {
                Debug.LogError("❌ Player no tiene PlayerHealth");
            }
        }
        else
        {
            Debug.LogError("❌ No se encontró el Player");
        }

        if (particles != null)
            Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
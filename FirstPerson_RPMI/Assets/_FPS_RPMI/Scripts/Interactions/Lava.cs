using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damage = 100;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
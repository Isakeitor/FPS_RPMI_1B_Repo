using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        DragObject dragObject = collision.collider.GetComponent<DragObject>();

        if (dragObject != null)
        {
            dragObject.TakeBulletHit();
        }

        Destroy(gameObject);
    }
}
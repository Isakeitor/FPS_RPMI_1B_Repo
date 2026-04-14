using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f); // se autodestruye si no choca
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
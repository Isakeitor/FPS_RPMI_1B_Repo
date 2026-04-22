using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GunSystem gun = other.GetComponentInChildren<GunSystem>();

            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
            }

            Destroy(gameObject);
        }
    }
}
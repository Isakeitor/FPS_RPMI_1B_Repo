using System.Collections;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int ammoAmount = 10;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 20f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private bool respawning = false;

    private Collider pickupCollider;
    private Renderer[] renderers;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        pickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (respawning)
            return;

        if (!other.CompareTag("Player"))
            return;

        GunSystem gun = other.GetComponentInChildren<GunSystem>();

        if (gun != null)
        {
            gun.AddAmmo(ammoAmount);

            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        respawning = true;

        // Desactivar el collider para que no se pueda recoger
        if (pickupCollider != null)
            pickupCollider.enabled = false;

        // Ocultar el objeto visualmente
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        // Volver a su posición original
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;

        // Mostrar el objeto
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        // Reactivar el collider
        if (pickupCollider != null)
            pickupCollider.enabled = true;

        respawning = false;
    }
}
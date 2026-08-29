using System.Collections;
using UnityEngine;

public class HealthPickup : Interactable
{
    [Header("Effects")]
    [SerializeField] private GameObject particles;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 50f;

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

    private void Start()
    {
        interactionText = "Heal";
    }

    public override void Interact()
    {
        if (respawning)
            return;

        base.Interact();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("❌ No se encontró el Player");
            return;
        }

        PlayerHealth ph = player.GetComponent<PlayerHealth>();

        if (ph == null)
        {
            Debug.LogError("❌ Player no tiene PlayerHealth");
            return;
        }

        ph.HealFull();

        if (particles != null)
        {
            Instantiate(
                particles,
                transform.position,
                Quaternion.identity
            );
        }

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        respawning = true;

        // Desactivar el collider
        if (pickupCollider != null)
            pickupCollider.enabled = false;

        // Ocultar el objeto
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        // Restaurar posición, rotación y escala
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;

        // Mostrar el objeto
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        // Reactivar collider
        if (pickupCollider != null)
            pickupCollider.enabled = true;

        respawning = false;
    }
}
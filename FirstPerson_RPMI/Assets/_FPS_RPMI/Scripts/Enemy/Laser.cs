using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser")]
    [SerializeField] private float activeTime = 3f;
    [SerializeField] private float inactiveTime = 2f;
    [SerializeField] private int damagePerSecond = 10;

    [Header("References")]
    [SerializeField] private Renderer laserRenderer;
    [SerializeField] private Collider laserCollider;

    private bool isActive = true;
    private float stateTimer;
    private float damageTimer;

    private void Start()
    {
        stateTimer = activeTime;

        if (laserRenderer == null)
            laserRenderer = GetComponent<Renderer>();

        if (laserCollider == null)
            laserCollider = GetComponent<Collider>();

        UpdateLaserState();
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            isActive = !isActive;

            stateTimer = isActive
                ? activeTime
                : inactiveTime;

            damageTimer = 0f;

            UpdateLaserState();
        }
    }

    private void UpdateLaserState()
    {
        if (laserRenderer != null)
            laserRenderer.enabled = isActive;

        if (laserCollider != null)
            laserCollider.enabled = isActive;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player == null)
            return;

        damageTimer += Time.deltaTime;

        if (damageTimer >= 1f)
        {
            player.TakeDamage(damagePerSecond);
            damageTimer = 0f;
        }
    }
}
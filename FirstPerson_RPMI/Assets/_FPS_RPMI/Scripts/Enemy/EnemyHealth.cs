using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health System Management")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health;

    [Header("Respawn")]
    [SerializeField] float respawnTime = 15f;

    [Header("Feedback Configuration")]
    [SerializeField] Material damagedMat;
    [SerializeField] GameObject deathVfx;
    [SerializeField] MeshRenderer enemyRend;

    [Header("Death Audio")]
    [SerializeField] AudioSource deathAudio;

    Material baseMat;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private bool respawning = false;

    private EnemyAIBase enemyAI;
    private NavMeshAgent agent;
    private Collider[] colliders;
    private Renderer[] renderers;

    private void Awake()
    {
        health = maxHealth;

        if (enemyRend != null)
        {
            baseMat = enemyRend.material;
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        enemyAI = GetComponent<EnemyAIBase>();
        agent = GetComponent<NavMeshAgent>();

        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        if (health <= 0 && !respawning)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (respawning)
            return;

        health -= damage;

        if (enemyRend != null && damagedMat != null)
        {
            enemyRend.material = damagedMat;

            CancelInvoke(nameof(ResetEnemyMaterial));
            Invoke(nameof(ResetEnemyMaterial), 0.1f);
        }
    }

    private void Die()
    {
        health = 0;
        respawning = true;

        // Sonido de muerte
        if (deathAudio != null)
        {
            deathAudio.Play();
        }

        // Detener la IA inmediatamente
        if (enemyAI != null)
        {
            enemyAI.SetDead(true);
        }

        // Detener el NavMeshAgent
        if (agent != null && agent.enabled)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }

        // VFX de muerte
        if (deathVfx != null)
        {
            deathVfx.SetActive(true);
            deathVfx.transform.position = transform.position;
        }

        // Desactivar colliders
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Ocultar modelo
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        // Volver a posición original
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;

        // Recuperar vida
        health = maxHealth;

        // Restaurar material
        if (enemyRend != null && baseMat != null)
        {
            enemyRend.material = baseMat;
        }

        // Ocultar VFX
        if (deathVfx != null)
        {
            deathVfx.SetActive(false);
        }

        // Reactivar colliders
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        // Mostrar modelo
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        // Reactivar NavMeshAgent
        if (agent != null)
        {
            agent.Warp(transform.position);
            agent.isStopped = false;
        }

        // Reactivar IA
        if (enemyAI != null)
        {
            enemyAI.SetDead(false);
        }

        respawning = false;
    }

    private void ResetEnemyMaterial()
    {
        if (enemyRend != null && !respawning)
        {
            enemyRend.material = baseMat;
        }
    }
}
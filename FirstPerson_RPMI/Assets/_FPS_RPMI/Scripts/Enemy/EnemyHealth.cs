using System.Collections;
using UnityEngine;

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

    Material baseMat;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private bool respawning = false;

    private void Awake()
    {
        health = maxHealth;

        baseMat = enemyRend.material;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
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

        enemyRend.material = damagedMat;

        Invoke(nameof(ResetEnemyMaterial), 0.1f);
    }

    private void Die()
    {
        health = 0;
        respawning = true;

        if (deathVfx != null)
        {
            deathVfx.SetActive(true);
            deathVfx.transform.position = transform.position;
        }

        gameObject.SetActive(false);

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;

        health = maxHealth;

        if (enemyRend != null)
        {
            enemyRend.material = baseMat;
        }

        if (deathVfx != null)
        {
            deathVfx.SetActive(false);
        }

        gameObject.SetActive(true);

        respawning = false;
    }

    private void ResetEnemyMaterial()
    {
        if (enemyRend != null)
        {
            enemyRend.material = baseMat;
        }
    }
}
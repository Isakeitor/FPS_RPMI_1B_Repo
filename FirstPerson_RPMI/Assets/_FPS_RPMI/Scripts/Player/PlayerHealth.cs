using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float overlayDuration = 0.2f;
    public float fadeSpeed = 5f;

    [Header("References")]
    public FPController playerController;

    Coroutine damageRoutine;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (damageOverlay != null)
        {
            Color c = damageOverlay.color;
            c.a = 0f;
            damageOverlay.color = c;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player HP: " + currentHealth);

        UpdateHealthUI();
        ShowDamageOverlay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 🔥 NUEVO → curar al máximo
    public void HealFull()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // 🔥 OPCIONAL → curar parcialmente
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
    }

    void ShowDamageOverlay()
    {
        if (damageOverlay == null) return;

        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        damageRoutine = StartCoroutine(DamageOverlayRoutine());
    }

    IEnumerator DamageOverlayRoutine()
    {
        Color c = damageOverlay.color;
        c.a = 0.5f;
        damageOverlay.color = c;

        yield return new WaitForSeconds(overlayDuration);

        while (damageOverlay.color.a > 0)
        {
            c = damageOverlay.color;
            c.a -= Time.deltaTime * fadeSpeed;
            damageOverlay.color = c;
            yield return null;
        }

        c.a = 0;
        damageOverlay.color = c;
    }

    void Die()
    {
        Debug.Log("PLAYER MUERTO");

        if (playerController != null)
        {
            playerController.canMove = false;
            playerController.canLook = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
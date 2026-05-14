using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    private bool isDead = false;

    [Header("HUD")]
    [SerializeField] private HealthHUD healthHUD;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        maxHealth = PlayerGameState.MaxHealth;
        currentHealth = PlayerGameState.CurrentHealth;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        UpdateHealthBar();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        PlayerGameState.CurrentHealth = currentHealth;

        Debug.Log("Player tomou dano. Vida atual: " + currentHealth);

        UpdateHealthBar();

        if (animator != null)
            animator.SetTrigger("hit");

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (healthHUD != null)
            healthHUD.UpdateHealth(currentHealth, maxHealth);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player morreu!");

        if (playerCollider != null)
            playerCollider.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (animator != null)
            animator.SetTrigger("death");

        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(1f);

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;

            float duration = 0.8f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

                spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
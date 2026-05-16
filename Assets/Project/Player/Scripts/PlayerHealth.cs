using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private bool isDead = false;
    private bool deathStarted = false;

    [Header("Slow Motion Death")]
    [SerializeField] private float deathSlowMotionScale = 0.3f;

    [Header("Invencibilidade")]
    [SerializeField] private float invincibilityDuration = 1.2f;
    [SerializeField] private float blinkInterval = 0.1f;
    private bool isInvincible = false;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackUpForce = 1.5f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float delayBeforeDeath = 0.15f;

    public bool IsKnockbacking { get; private set; }

    [Header("HUD")]
    [SerializeField] private HealthHUD healthHUD;

    [Header("Transição ao morrer")]
    [SerializeField] private SceneTransition sceneTransition;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Reseta a vida ao reentrar na cena
        PlayerGameState.CurrentHealth = PlayerGameState.MaxHealth;

        maxHealth = PlayerGameState.MaxHealth;
        currentHealth = maxHealth;
        PlayerGameState.CurrentHealth = currentHealth;

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
        if (isDead || isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerGameState.CurrentHealth = currentHealth;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerGameState.CurrentHealth = 0;

            isDead = true;

            UpdateHealthBar();

            StartCoroutine(DeathRoutine());

            return;
        }

        if (animator != null)
            animator.SetTrigger("hit");

        StartCoroutine(KnockbackRoutine());
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        if (playerCollider != null)
            playerCollider.enabled = false;

        StartCoroutine(KnockbackRoutine());

        yield return new WaitForSeconds(delayBeforeDeath);

        Die();
    }

    private IEnumerator KnockbackRoutine()
    {
        IsKnockbacking = true;

        if (rb != null)
        {
            float direction = spriteRenderer != null && spriteRenderer.flipX ? 1f : -1f;

            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = new Vector2(direction * knockbackForce, knockbackUpForce);
        }

        yield return new WaitForSeconds(knockbackDuration);

        IsKnockbacking = false;
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float elapsed = 0f;

        while (elapsed < invincibilityDuration && !isDead)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvincible = false;
    }

    private void UpdateHealthBar()
    {
        if (healthHUD != null)
            healthHUD.UpdateHealth(currentHealth, maxHealth);
    }

    public void Die()
    {
        if (deathStarted) return;
        deathStarted = true;

        Time.timeScale = deathSlowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (playerCollider != null)
            playerCollider.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (animator != null)
        {
            animator.ResetTrigger("hit");
            animator.SetTrigger("death");
        }

        StartCoroutine(DeathBlinkRoutine());
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator DeathBlinkRoutine()
    {
        float elapsed = 0f;
        float duration = 1.2f;

        while (elapsed < duration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSecondsRealtime(blinkInterval);
            elapsed += blinkInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSecondsRealtime(1.8f);

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            float duration = 1.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        // Reseta o timeScale antes de transicionar
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Volta para a cena atual
        string currentScene = SceneManager.GetActiveScene().name;

        if (sceneTransition != null)
            sceneTransition.LoadScene(currentScene);
        else
            SceneManager.LoadScene(currentScene);
    }
}
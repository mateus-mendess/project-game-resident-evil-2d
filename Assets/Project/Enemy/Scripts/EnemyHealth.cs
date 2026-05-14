using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Referências")]
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private GameObject healthBarObject;

    private Animator animator;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private EnemyDrop enemyDrop;

    [Header("Áudio")]
    public AudioClip damageSound;
    public AudioClip deathSound;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        enemyDrop = GetComponent<EnemyDrop>();

        if (healthBarObject != null)
            healthBarObject.SetActive(false);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBarObject != null)
            healthBarObject.SetActive(true);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            if (healthBarObject != null)
                healthBarObject.SetActive(false);

            Die();
            return;
        }

        if (animator != null)
        {
            animator.ResetTrigger("hurt");
            animator.SetTrigger("hurt");
        }

        if (audioSource != null && damageSound != null)
            audioSource.PlayOneShot(damageSound);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
            movement.enabled = false;

        EnemyCombat combat = GetComponent<EnemyCombat>();
        if (combat != null)
            combat.enabled = false;

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.ResetTrigger("hurt");
            animator.ResetTrigger("attack");
            animator.SetBool("isWalking", false);
            animator.SetTrigger("dead");
        }

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);

        if (enemyDrop != null)
            enemyDrop.DropItems();

        if (spriteRenderer == null)
        {
            Destroy(gameObject);
            yield break;
        }

        Color color = spriteRenderer.color;
        float duration = 0.8f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
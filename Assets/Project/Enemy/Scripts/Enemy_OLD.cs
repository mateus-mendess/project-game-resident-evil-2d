using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int health = 50;
    public float moveSpeed = 2f;
    public int attackDamage = 10;

    [Header("Distâncias")]
    public float detectionRange = 5f;
    public float attackRange = 1.2f;

    [Header("Tempo")]
    public float attackCooldown = 1.5f;

    [Header("Áudio")]
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip attackSound;

    private Animator animator;
    private bool isDead = false;
    private bool canAttack = true;

    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

        private void Update()
    {
        if (player == null || isDead) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.IsDead())
        {
            StopMoving();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        FlipTowardsPlayer();

        if (distanceToPlayer <= attackRange)
        {
            StopMoving();

            if (canAttack)
            {
                Attack();
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void ChasePlayer()
    {
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void StopMoving()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Attack()
    {
        canAttack = false;

        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        StartCoroutine(AttackCooldownRoutine());
    }

        public void DealDamageToPlayer()
    {
        if (player == null || isDead) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null || playerHealth.IsDead()) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Inimigo acertou o player");
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void FlipTowardsPlayer()
    {
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Inimigo tomou dano. Vida atual: " + health);

        if (health <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
        {
            animator.ResetTrigger("hurt");
            animator.SetTrigger("hurt");
        }

        if (audioSource != null && damageSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(damageSound);
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Inimigo morreu!");

        StopAllCoroutines();

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (animator != null)
        {
            animator.ResetTrigger("hurt");
            animator.ResetTrigger("attack");
            animator.SetBool("isWalking", false);
            animator.SetTrigger("dead");
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        StartCoroutine(DeathRoutine());
    }
    
    public void StartFadeOut()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
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

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.0f); // ajuste conforme a duração da animação de morte
        StartFadeOut();
    }
}
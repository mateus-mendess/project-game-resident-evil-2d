using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackRange = 1.2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private Transform player;
    private Animator animator;
    private bool canAttack = true;
    private AudioSource audioSource;

    public AudioClip attackSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null && health.IsDead()) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;

        animator?.SetTrigger("attack");

        if (audioSource && attackSound)
            audioSource.PlayOneShot(attackSound);

        StartCoroutine(Cooldown());
    }

    public void DealDamage()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
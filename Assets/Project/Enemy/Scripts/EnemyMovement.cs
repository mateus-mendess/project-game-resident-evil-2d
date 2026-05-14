using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float chaseSpeed = 2f;

    [Header("Patrulha")]
    [SerializeField] private float patrolDistance = 2f;

    [Header("Detecção")]
    [SerializeField] private float detectionRange = 8f;

    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyCombat combat;

    private Vector2 startPosition;
    private int patrolDirection = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        combat = GetComponent<EnemyCombat>();

        startPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null && health.IsDead())
        {
            Stop();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        float stopDistance = combat != null ? combat.attackRange : 0.5f;

        if (distance <= stopDistance)
        {
            FacePlayer();
            Stop();
        }
        else if (distance <= detectionRange)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        float leftLimit = startPosition.x - patrolDistance;
        float rightLimit = startPosition.x + patrolDistance;

        transform.Translate(Vector2.right * patrolDirection * patrolSpeed * Time.deltaTime);

        if (transform.position.x >= rightLimit)
            patrolDirection = -1;
        else if (transform.position.x <= leftLimit)
            patrolDirection = 1;

        FlipByDirection(patrolDirection);

        animator?.SetBool("isWalking", true);
    }

    private void Chase()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        Vector2 target = new Vector2(player.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            chaseSpeed * Time.deltaTime
        );

        FlipByDirection((int)direction);

        animator?.SetBool("isWalking", true);
    }

    private void Stop()
    {
        animator?.SetBool("isWalking", false);
    }

    private void FacePlayer()
    {
        if (player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        FlipByDirection((int)direction);
    }

    private void FlipByDirection(int direction)
    {
        if (spriteRenderer == null || direction == 0) return;

        spriteRenderer.flipX = direction < 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Vector3 left = new Vector3(transform.position.x - patrolDistance, transform.position.y, transform.position.z);
        Vector3 right = new Vector3(transform.position.x + patrolDistance, transform.position.y, transform.position.z);
        Gizmos.DrawLine(left, right);
    }
}
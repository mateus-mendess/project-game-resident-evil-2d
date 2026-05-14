using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Vector2 movement;
    private PlayerCombat combat;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        combat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        animator.SetFloat("speed", Mathf.Abs(movement.x));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (combat != null && combat.CanShoot())
            {
                animator.SetTrigger("shoot");
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<PlayerHealth>().TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<PlayerHealth>().Die();
        }

        HandleFlip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    void HandleFlip()
    {
        if (movement.x > 0)
        {
            sprite.flipX = false;
            combat.SetAttackDirection(1);
        }
        else if (movement.x < 0)
        {
            sprite.flipX = true;
            combat.SetAttackDirection(-1);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

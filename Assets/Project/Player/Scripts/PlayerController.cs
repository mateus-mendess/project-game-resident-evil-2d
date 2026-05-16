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
    private PlayerHealth playerHealth;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        combat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.IsDead()) return;

        // Movimento horizontal via GetAxis (já captura A/D e Arrow Left/Right)
        movement.x = Input.GetAxis("Horizontal");

        if (combat != null && combat.IsReloading() && Mathf.Abs(movement.x) > 0.01f)
            combat.CancelReload();

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        animator.SetFloat("speed", Mathf.Abs(movement.x));

        // Pular — W ou Arrow Up
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            if (combat != null)
                combat.CancelReload();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("jump");
        }

        // Bater — L ou C
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.C))
        {
            if (combat != null)
                combat.CancelReload();

            animator.SetTrigger("attack");
        }

        // Atirar — J ou Z
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
        {
            if (combat != null)
            {
                combat.CancelReload();

                if (combat.CanShoot())
                    animator.SetTrigger("shoot");
            }
        }

        // Carregar arma — K ou X
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
        {
            if (combat != null)
                combat.Reload();
        }

        HandleFlip();
    }

    void FixedUpdate()
    {
        if (playerHealth != null && playerHealth.IsDead()) return;
        if (playerHealth != null && playerHealth.IsKnockbacking) return;

        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    void HandleFlip()
    {
        if (combat == null) return;

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
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
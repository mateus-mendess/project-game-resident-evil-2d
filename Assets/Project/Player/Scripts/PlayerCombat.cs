using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Ataque Corpo a Corpo")]
    public Transform attackPoint;
    public float attackRange = 0.2f;
    public LayerMask enemyLayer;
    public int attackDamage = 5;

    [Header("Áudio")]
    public AudioClip attackSound;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    [Header("Munição")]
    [SerializeField] private int maxAmmo = 12;
    [SerializeField] private int currentAmmo = 12;

    [Header("Recarga")]
    private bool isReloading = false;

    [Header("HUD")]
    [SerializeField] private AmmoHUD ammoHUD;

    private Animator animator;
    private float attackPointX;
    private int direction = 1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        maxAmmo = PlayerGameState.MaxAmmo;
        currentAmmo = PlayerGameState.CurrentAmmo;

        if (attackPoint != null)
            attackPointX = Mathf.Abs(attackPoint.localPosition.x);

        UpdateAttackPoint();

        if (ammoHUD != null)
            ammoHUD.UpdateAmmo(currentAmmo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public void SetAttackDirection(int dir)
    {
        direction = dir;
        UpdateAttackPoint();
    }

    private void UpdateAttackPoint()
    {
        if (attackPoint == null) return;

        attackPoint.localPosition = new Vector3(
            attackPointX * direction,
            attackPoint.localPosition.y,
            attackPoint.localPosition.z
        );
    }

    public void PerformAttack()
    {
        if (isReloading) return;

        if (audioSource != null && attackSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(attackSound);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

            if (enemyHealth == null)
                enemyHealth = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
                enemyHealth.TakeDamage(attackDamage);
        }
    }

    public void Shoot()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            Debug.Log("Sem munição!");
            return;
        }

        currentAmmo--;
        PlayerGameState.CurrentAmmo = currentAmmo;

        if (ammoHUD != null)
            ammoHUD.UpdateAmmo(currentAmmo);

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.right * direction,
            10f,
            enemyLayer
        );

        if (hit.collider != null)
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth == null)
                enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
                enemyHealth.TakeDamage(10);
        }
    }

    public bool CanShoot()
    {
        return !isReloading && currentAmmo > 0;
    }

    public void Reload()
    {
        if (isReloading) return;

        if (currentAmmo >= maxAmmo)
        {
            Debug.Log("Munição já está cheia!");
            return;
        }

        isReloading = true;

        if (animator != null)
            animator.SetTrigger("reload");

        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);
    }

    public void CancelReload()
    {
        if (!isReloading) return;

        isReloading = false;

        if (animator != null)
        {
            animator.ResetTrigger("reload");
            animator.CrossFade("Idle", 0.05f);
        }

        Debug.Log("Recarga cancelada.");
    }

    public void FinishReload()
    {
        if (!isReloading) return;

        currentAmmo = maxAmmo;
        PlayerGameState.CurrentAmmo = currentAmmo;

        if (ammoHUD != null)
            ammoHUD.UpdateAmmo(currentAmmo);

        isReloading = false;

        Debug.Log("Recarga finalizada.");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
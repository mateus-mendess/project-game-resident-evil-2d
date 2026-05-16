using UnityEngine;

public class EnemyWall : MonoBehaviour
{
    [Header("Seta")]
    [SerializeField] private ArrowIndicator arrowIndicator;

    private Collider2D wallCollider;

    private void Start()
    {
        wallCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (wallCollider == null || !wallCollider.enabled) return;

        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();

        bool allDead = true;

        foreach (EnemyHealth enemy in enemies)
        {
            if (!enemy.IsDead())
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            wallCollider.enabled = false;

            if (arrowIndicator != null)
                arrowIndicator.Show();
        }
    }
}
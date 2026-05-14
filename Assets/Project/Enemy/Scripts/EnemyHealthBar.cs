using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Transform fill;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        float healthPercent = currentHealth / maxHealth;
        healthPercent = Mathf.Clamp01(healthPercent);

        fill.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}
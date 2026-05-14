using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private Image healthBarFull;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBarFull.fillAmount = (float)currentHealth / maxHealth;
    }
}
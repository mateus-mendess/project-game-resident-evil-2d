using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private Image healthBarFull;

    [Header("Munição")]
    [SerializeField] private TMP_Text ammoText;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBarFull.fillAmount = (float) currentHealth / maxHealth;
    }

    public void UpdateAmmo(int currentAmmo)
    {
        ammoText.text = currentAmmo.ToString();
    }
}
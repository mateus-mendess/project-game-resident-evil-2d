using TMPro;
using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    public void UpdateAmmo(int currentAmmo)
    {
        ammoText.text = currentAmmo.ToString();
    }
}
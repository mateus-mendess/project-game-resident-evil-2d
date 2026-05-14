using TMPro;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private int coins;

    private void Start()
    {
        coins = PlayerGameState.Coins;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        PlayerGameState.Coins = coins;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }
}
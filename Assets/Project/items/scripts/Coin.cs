using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerCoins playerCoins = collision.GetComponent<PlayerCoins>();

        if (playerCoins != null)
            playerCoins.AddCoins(value);

        Destroy(gameObject);
    }
}
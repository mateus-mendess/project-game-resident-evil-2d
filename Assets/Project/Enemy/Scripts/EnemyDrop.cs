using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Drop Offset")]
    [SerializeField] private Vector2 dropOffset;

    public void DropItems()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("Coin Prefab não foi configurado no EnemyDrop.");
            return;
        }

        Vector3 dropPosition = transform.position + (Vector3)dropOffset;

        Instantiate(coinPrefab, dropPosition, Quaternion.identity);
    }
}
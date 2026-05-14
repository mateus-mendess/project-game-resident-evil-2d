using System.Collections;
using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Config")]
    [SerializeField] private int enemyAmount = 1;
    [SerializeField] private float spawnDelay = 1.5f;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSpawned) return;
        if (!collision.CompareTag("Player")) return;

        hasSpawned = true;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];

            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
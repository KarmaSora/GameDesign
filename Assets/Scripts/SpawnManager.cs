using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [Tooltip("List of enemy prefabs to spawn from. A random one will be chosen for each enemy.")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Powerup Spawning")]
    [Tooltip("List of powerup prefabs to spawn from. A random one will be chosen each wave.")]
    [SerializeField] private GameObject[] powerUpPrefabs;

    [Header("Spawn Settings")]
    [Tooltip("Radius around this GameObject's position where enemies and powerups can spawn.")]
    [SerializeField] private float spawnRange = 9f;

    [Tooltip("Maximum number of waves to spawn.")]
    [SerializeField] private int maxWaves = 5;

    [Header("Runtime Info (read-only)")]
    public int enemyCount;   // Only enemies this spawner has spawned
    public int waveNumber = 1;

    // Internal list tracking ONLY enemies spawned by THIS SpawnManager
    private readonly List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isLevelComplete = false;

    private void Start()
    {
        SpawnEnemyWave(waveNumber);
        SpawnRandomPowerup();
    }

    private void Update()
    {
        // Clean up any entries where the enemy has been destroyed (becomes null)
        spawnedEnemies.RemoveAll(e => e == null);

        // Count how many enemies spawned by THIS spawner are still alive
        enemyCount = spawnedEnemies.Count;

        if (enemyCount == 0)
        {
            if (waveNumber <= maxWaves)
            {
                SpawnEnemyWave(waveNumber);
                SpawnRandomPowerup();
                waveNumber++;
            }
            else if(isLevelComplete ==false)
            {
                isLevelComplete = true;
                Debug.Log($"SpawnManager ({name}): All waves completed!");
            }
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn = 3)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning($"SpawnManager ({name}): No enemyPrefabs assigned!");
            return;
        }

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Pick a random enemy prefab from the list
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            if (enemyPrefab == null)
            {
                Debug.LogWarning($"SpawnManager ({name}): Selected enemy prefab is null, skipping spawn.");
                continue;
            }

            Vector3 spawnPos = GenerateSpawnPos();
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);

            // Track this instance so only enemies from THIS spawner are counted
            spawnedEnemies.Add(enemyInstance);
        }
    }

    // Generates a random position within 'spawnRange' around THIS object's position
    private Vector3 GenerateSpawnPos()
    {
        float spawnXOffset = Random.Range(-spawnRange, spawnRange);
        float spawnZOffset = Random.Range(-spawnRange, spawnRange);

        Vector3 center = transform.position;

        return new Vector3(
            center.x + spawnXOffset,
            center.y,
            center.z + spawnZOffset
        );
    }

    private void SpawnRandomPowerup()
    {

        if (powerUpPrefabs.Length == 0) return;
        if (powerUpPrefabs == null )
        {

            Debug.LogWarning($"SpawnManager ({name}): No powerUpPrefabs assigned!");
            return;
        }

        int index = Random.Range(0, powerUpPrefabs.Length);
        GameObject selectedPowerup = powerUpPrefabs[index];

        if (selectedPowerup == null)
        {
            Debug.LogWarning($"SpawnManager ({name}): Selected powerup prefab is null at index {index}");
            return;
        }

        Vector3 spawnPos = GenerateSpawnPos();
        Instantiate(selectedPowerup, spawnPos, selectedPowerup.transform.rotation);
    }

    // Draw a wire sphere in the Scene view to visualize the spawn range around this object
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}

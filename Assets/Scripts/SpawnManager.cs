using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject[] powerUpPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRadius = 2f;   // Random radius around each spawn point

    public int waveNumber = 1;
    private int enemyCount;

    void Start()
    {
        spawnEnemyWave(waveNumber);
        SpawnRandomPowerup();
    }

    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        if (enemyCount == 0)
        {
            spawnEnemyWave(waveNumber++);
            SpawnRandomPowerup();
        }
    }

    void spawnEnemyWave(int enemiesToSpawn = 3)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GetSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnRandomPowerup()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            Debug.LogWarning("SpawnManager: No powerUpPrefabs assigned!");
            return;
        }

        int index = Random.Range(0, powerUpPrefabs.Length);
        GameObject selectedPowerup = powerUpPrefabs[index];

        if (selectedPowerup == null)
        {
            Debug.LogWarning("SpawnManager: Selected powerup prefab is null at index " + index);
            return;
        }

        Instantiate(selectedPowerup, GetSpawnPosition(), selectedPowerup.transform.rotation);
    }

    // -------------------------------
    // SPAWN POINT SYSTEM
    // -------------------------------

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: No spawn points assigned!");
            return null;
        }

        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index];
    }

    // Returns a position within 'spawnRadius' meters of a random spawn point
    private Vector3 GetSpawnPosition()
    {
        Transform t = GetRandomSpawnPoint();
        if (t == null)
        {
            return Vector3.zero;
        }

        // 2D random offset on XZ plane
        Vector2 circle = Random.insideUnitCircle * spawnRadius;

        return new Vector3(
            t.position.x + circle.x,
            t.position.y,
            t.position.z + circle.y
        );
    }
}

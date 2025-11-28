using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public GameObject[] powerUpPrefabs;

    public float spawnRange = 9;

    public int enemyCount;

    public int waveNumber = 1;

    [SerializeField] private int maxWaves = 5;   // NEW — limit number of waves

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
            if (waveNumber <= maxWaves)
            {
                spawnEnemyWave(waveNumber);
                SpawnRandomPowerup();
                waveNumber++;
            }
            else
            {
                // No more waves beyond the configured maximum
                Debug.Log("All waves completed!");
            }
        }
    }

    void spawnEnemyWave(int enemiesTOSpawn = 3)
    {
        for (int i = 0; i < enemiesTOSpawn; i++)
        {
            Instantiate(enemyPrefab, generateSpawnPos(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 generateSpawnPos()
    {
        float spawnXPos = Random.Range(-spawnRange, spawnRange);
        float spawnZPos = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnXPos, 0, spawnZPos);
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

        Instantiate(selectedPowerup, generateSpawnPos(), selectedPowerup.transform.rotation);
    }
}

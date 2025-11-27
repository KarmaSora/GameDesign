using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemyPrefab;

    public GameObject powerUpPrefab;

    public float spawnRange = 9;

    public int enemyCount;

    public int waveNumber = 1;

    void Start()
    {

        spawnEnemyWave(waveNumber);
        Instantiate(powerUpPrefab, generateSpawnPos(), powerUpPrefab.transform.rotation);



    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        if (enemyCount == 0)
        {
            spawnEnemyWave(waveNumber++);
            Instantiate(powerUpPrefab, generateSpawnPos(), powerUpPrefab.transform.rotation);
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
        Vector3 randomPos = new Vector3(spawnXPos, 0, spawnZPos);

        return randomPos;
    }
}

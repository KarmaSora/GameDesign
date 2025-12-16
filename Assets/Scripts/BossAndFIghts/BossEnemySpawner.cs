using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("BoxCollider used as the detection range. Typically placed on a child GameObject.")]
    [SerializeField] private BoxCollider detectionZone;

    [Header("Spawn Settings")]
    [Tooltip("Enemy prefabs to randomly choose from when spawning.")]
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    [Tooltip("Time in seconds between spawns while the player is in range.")]
    [SerializeField] private float spawnInterval = 5f;

    [Tooltip("Optional spawn points. If empty, the boss position will be used.")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Ground Snap Settings")]
    [Tooltip("Snap the spawn position down to the ground using a raycast.")]
    [SerializeField] private bool snapToGround = true;

    [Tooltip("How high above the spawn position the raycast should start.")]
    [SerializeField] private float groundCheckHeight = 5f;

    [Tooltip("Maximum distance the raycast can travel downward to find the ground.")]
    [SerializeField] private float groundCheckDistance = 20f;

    [Tooltip("Layers considered as ground/platform for the raycast.")]
    [SerializeField] private LayerMask groundLayerMask = ~0; // default: all layers

    [Header("Tracking Spawned Enemies")]
    [Tooltip("List of enemies spawned by this boss. Do not modify in Inspector at runtime.")]
    [SerializeField] private List<GameObject> spawnedEnemies = new List<GameObject>();

    private float spawnTimer;

    private void Awake()
    {
        if (detectionZone == null)
        {
            detectionZone = GetComponentInChildren<BoxCollider>();

            if (detectionZone == null)
                Debug.LogWarning($"{nameof(BossEnemySpawner)} on {name} has no detectionZone assigned.");
        }
    }

    private void Update()
    {
        if (detectionZone == null)
            return;

        if (!HasAtLeastOneValidEnemyPrefab())
            return;

        if (!IsPlayerInRange())
        {
            spawnTimer = 0f;
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private bool HasAtLeastOneValidEnemyPrefab()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
            return false;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (enemyPrefabs[i] != null)
                return true;
        }

        return false;
    }

    private bool IsPlayerInRange()
    {
        Vector3 center = detectionZone.bounds.center;
        Vector3 halfExtents = detectionZone.bounds.extents;
        Quaternion rotation = detectionZone.transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation);

        foreach (Collider c in hits)
        {
            if (c.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = GetRandomEnemyPrefab();
        if (prefabToSpawn == null)
            return;

        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int i = Random.Range(0, spawnPoints.Length);
            Transform point = spawnPoints[i];
            spawnPosition = point.position;
            spawnRotation = point.rotation;
        }
        else
        {
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }

        if (snapToGround)
        {
            spawnPosition = GetGroundPosition(spawnPosition);
        }

        GameObject enemyInstance = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
        spawnedEnemies.Add(enemyInstance);
    }

    private GameObject GetRandomEnemyPrefab()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
            return null;

        // Try a few random picks first (fast, avoids allocations).
        int tries = enemyPrefabs.Count;
        for (int t = 0; t < tries; t++)
        {
            int index = Random.Range(0, enemyPrefabs.Count);
            GameObject candidate = enemyPrefabs[index];
            if (candidate != null)
                return candidate;
        }

        // Fallback: deterministic scan for first valid entry.
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (enemyPrefabs[i] != null)
                return enemyPrefabs[i];
        }

        return null;
    }

    private Vector3 GetGroundPosition(Vector3 originalPosition)
    {
        Vector3 rayStart = originalPosition + Vector3.up * groundCheckHeight;
        Ray ray = new Ray(rayStart, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayerMask))
        {
            return hit.point;
        }

        return originalPosition;
    }

    /// <summary>
    /// Called to kill all enemies spawned by this boss.
    /// </summary>
    public void KillAllSpawnedEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            GameObject enemy = spawnedEnemies[i];


            if (enemy != null)
            {


                Destroy(enemy);
                

            }
        }

        spawnedEnemies.Clear();
    }

    private void OnDestroy()
    {
        KillAllSpawnedEnemies();
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionZone != null)
        {
            Gizmos.matrix = detectionZone.transform.localToWorldMatrix;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(detectionZone.center, detectionZone.size);
        }

        if (spawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform t in spawnPoints)
            {
                if (t != null)
                    Gizmos.DrawSphere(t.position, 0.2f);
            }
        }
    }
}

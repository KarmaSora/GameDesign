using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("BoxCollider used as the detection range. Typically placed on a child GameObject.")]
    [SerializeField] private BoxCollider detectionZone;

    [Header("Spawn Settings")]
    [Tooltip("Enemy prefab to spawn.")]
    [SerializeField] private GameObject enemyPrefab;

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
        if (enemyPrefab == null || detectionZone == null)
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

        // Snap to ground/platform
        if (snapToGround)
        {
            spawnPosition = GetGroundPosition(spawnPosition);
        }

        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, spawnRotation);

        // Track this enemy as one of the boss minions
        spawnedEnemies.Add(enemyInstance);
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
        // When boss gets destroyed, also kill all its minions
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

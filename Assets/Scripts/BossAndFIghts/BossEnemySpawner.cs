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
        // Get BoxCollider world space data
        Vector3 center = detectionZone.bounds.center;
        Vector3 halfExtents = detectionZone.bounds.extents;
        Quaternion rotation = detectionZone.transform.rotation;

        // Find all colliders overlapping the box
        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation);

        // Check if any collider belongs to the Player
        foreach (Collider c in hits)
        {
            if (c.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void SpawnEnemy()
    {
        Vector3 pos;
        Quaternion rot;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int i = Random.Range(0, spawnPoints.Length);
            pos = spawnPoints[i].position;
            rot = spawnPoints[i].rotation;
        }
        else
        {
            pos = transform.position;
            rot = transform.rotation;
        }

        GameObject enemyInstance = Instantiate(enemyPrefab, pos, rot);

        // If this spawner also has a PowerupDropOnSpawn component, let it handle drops
        PowerupDropOnSpawn dropper = GetComponent<PowerupDropOnSpawn>();
        if (dropper != null)
        {
            dropper.TrySpawnPowerup(pos);
        }
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

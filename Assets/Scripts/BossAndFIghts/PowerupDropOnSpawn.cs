using UnityEngine;

public class PowerupDropOnSpawn : MonoBehaviour
{
    [Header("Powerup Drop Settings")]
    [Tooltip("A list of powerup prefabs that can be dropped.")]
    [SerializeField] private GameObject[] powerupPrefabs;

    [Tooltip("Chance per spawn that ANY powerup will drop. 0 = never, 1 = always.")]
    [Range(0f, 1f)]
    [SerializeField] private float dropChancePerSpawn = 0.2f;

    [Tooltip("Vertical offset for where the powerup should appear relative to the spawn position.")]
    [SerializeField] private float verticalOffset = 0.5f;

    /// <summary>
    /// Call this from your spawner script whenever you spawn an enemy (or anything that can drop a powerup).
    /// </summary>
    /// <param name="spawnPosition">World position where the powerup should appear (usually enemy spawn position).</param>
    public void TrySpawnPowerup(Vector3 spawnPosition)
    {
        if (powerupPrefabs == null || powerupPrefabs.Length == 0)
            return;

        // Roll for drop
        float roll = Random.value;
        if (roll > dropChancePerSpawn)
            return;

        // Choose a random powerup
        int index = Random.Range(0, powerupPrefabs.Length);
        GameObject powerupPrefab = powerupPrefabs[index];

        if (powerupPrefab == null)
            return;

        Vector3 dropPosition = spawnPosition + Vector3.up * verticalOffset;

        Instantiate(powerupPrefab, dropPosition, Quaternion.identity);
    }
}

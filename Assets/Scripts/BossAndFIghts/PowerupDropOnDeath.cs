using UnityEngine;

public class PowerupDropOnDeath : MonoBehaviour
{
    [Header("Powerup Drop Settings")]
    [Tooltip("Powerup prefabs that can be dropped on death.")]
    [SerializeField] private GameObject[] powerupPrefabs;

    [Tooltip("Chance that a powerup will drop when this object dies. 0 = never, 1 = always.")]
    [Range(0f, 1f)]
    [SerializeField] private float dropChanceOnDeath = 0.3f;

    [Header("Ground Snap Settings")]
    [Tooltip("Snap the drop position to the ground using a downward raycast.")]
    [SerializeField] private bool snapToGround = true;

    [Tooltip("Raycast height above the drop position.")]
    [SerializeField] private float groundCheckHeight = 5f;

    [Tooltip("Maximum raycast distance downward.")]
    [SerializeField] private float groundCheckDistance = 20f;

    [Tooltip("Layers considered as ground/platform for snapping.")]
    [SerializeField] private LayerMask groundLayerMask = ~0;

    private bool hasDropped = false;

    /// <summary>
    /// Call this from your death logic BEFORE destroying or disabling the object.
    /// </summary>
    public void DropNow()
    {
        if (hasDropped)
            return;

        hasDropped = true;

        if (powerupPrefabs == null || powerupPrefabs.Length == 0)
            return;

        // Roll drop chance
        float roll = Random.value;
        if (roll > dropChanceOnDeath)
            return;

        // Pick a random powerup
        int index = Random.Range(0, powerupPrefabs.Length);
        GameObject powerupPrefab = powerupPrefabs[index];
        if (powerupPrefab == null)
            return;

        // EXACT enemy position
        Vector3 dropPosition = transform.position;

        // Snap to ground if enabled
        if (snapToGround)
        {
            dropPosition = GetGroundPosition(dropPosition);
        }

        Instantiate(powerupPrefab, dropPosition, Quaternion.identity);
    }


    private Vector3 GetGroundPosition(Vector3 originalPosition)
    {
        Vector3 rayStart = originalPosition + Vector3.up * groundCheckHeight;
        Ray ray = new Ray(rayStart, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayerMask))
        {
            return hit.point;
        }

        return originalPosition; // fallback
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}

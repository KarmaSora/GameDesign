using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color triggeredColor = Color.red;   // Color when player steps on it
    [SerializeField] private float fallDelay = 1.5f;             // Time before the platform starts falling

    [Header("Respawn (optional)")]
    [SerializeField] private bool respawn = false;               // Enable/disable respawn
    [SerializeField] private float respawnDelay = 3f;            // Time after fall before respawn

    [Header("References")]
    [SerializeField] private Renderer groundRenderer;            // Renderer for colour change
    [SerializeField] private Rigidbody rb;                       // Rigidbody that will fall

    private Color originalColor;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool hasTriggered = false;

    private void Awake()
    {
        // Cache starting transform for respawn
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Auto-assign components if not set in Inspector
        if (groundRenderer == null)
            groundRenderer = GetComponentInChildren<Renderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (groundRenderer != null)
            originalColor = groundRenderer.material.color;

        if (rb != null)
        {
            // Platform starts frozen in place
            rb.isKinematic = true;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react once, and only to the player
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        // Change colour immediately
        if (groundRenderer != null)
            groundRenderer.material.color = triggeredColor;

        // Start countdown to falling
        Invoke(nameof(StartFalling), fallDelay);
    }

    private void StartFalling()
    {
        if (rb != null)
            rb.isKinematic = false;   // Enable physics so it falls

        if (respawn)
            Invoke(nameof(RespawnPlatform), respawnDelay);
    }

    private void RespawnPlatform()
    {
        if (rb == null) return;

        // Stop physics and reset transform
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = startRotation;

        // Restore original colour
        if (groundRenderer != null)
            groundRenderer.material.color = originalColor;

        hasTriggered = false;
    }
}

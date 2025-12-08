using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color triggeredColor = Color.red;
    [SerializeField] private float fallDelay = 1.5f;

    [Header("Respawn (optional)")]
    [SerializeField] private bool respawn = false;
    [SerializeField] private float respawnDelay = 3f;

    [Header("References")]
    [SerializeField] private Renderer groundRenderer;
    [SerializeField] private Rigidbody rb;

    private Color originalColor;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool hasTriggered = false;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (groundRenderer == null)
            groundRenderer = GetComponentInChildren<Renderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (groundRenderer != null)
            originalColor = groundRenderer.material.color;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (groundRenderer != null)
            groundRenderer.material.color = triggeredColor;

        Invoke(nameof(StartFalling), fallDelay);
    }

    private void StartFalling()
    {
        if (rb != null)
            rb.isKinematic = false;   // now physics takes over

        if (respawn)
            Invoke(nameof(RespawnPlatform), respawnDelay);
    }

    private void RespawnPlatform()
    {
        if (rb == null) return;

        // First stop physics while still dynamic
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Then freeze it again
        rb.isKinematic = true;

        transform.position = startPosition;
        transform.rotation = startRotation;

        if (groundRenderer != null)
            groundRenderer.material.color = originalColor;

        hasTriggered = false;
    }
}

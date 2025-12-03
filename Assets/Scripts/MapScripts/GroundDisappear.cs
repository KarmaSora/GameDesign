using System.Collections;
using UnityEngine;

public class GroundDisappear : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color triggeredColor = Color.red;   // Colour when player steps on it
    [SerializeField] private float fallDelay = 2f;               // Seconds before platform starts falling
    [SerializeField] private float respawnDelay = 3f;            // Seconds after it starts falling before it respawns

    [Header("References")]
    [SerializeField] private Renderer groundRenderer;            // Renderer to change colour
    [SerializeField] private Rigidbody rb;                       // Rigidbody to make it fall

    private bool hasTriggered = false;
    private Color originalColor;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        // Cache start transform so we can respawn here
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Assign renderer automatically if not set
        if (groundRenderer == null)
            groundRenderer = GetComponentInChildren<Renderer>();

        if (groundRenderer != null)
            originalColor = groundRenderer.material.color;

        // Assign rigidbody automatically if not set
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
                rb = GetComponentInChildren<Rigidbody>();
        }

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;   // start frozen
        }
        else
        {
            Debug.LogWarning("GroundDisappear: No Rigidbody found on this platform.");
        }
    }

    // For setups where the player uses a Rigidbody and stands on a solid collider
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        TryTrigger();
    }

    // For setups where the player uses a CharacterController or you use a trigger volume
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        TryTrigger();
    }

    private void TryTrigger()
    {
        if (hasTriggered)
            return;

        hasTriggered = true;
        ChangeColour();
        StartCoroutine(FallAndRespawnRoutine());
    }

    private void ChangeColour()
    {
        if (groundRenderer != null)
            groundRenderer.material.color = triggeredColor;
    }

    private IEnumerator FallAndRespawnRoutine()
    {
        // Wait before starting to fall
        yield return new WaitForSeconds(fallDelay);

        StartFalling();

        // Stay fallen for a while
        yield return new WaitForSeconds(respawnDelay);

        ResetPlatform();
    }

    private void StartFalling()
    {
        if (rb == null)
            return;

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void ResetPlatform()
    {
        if (rb == null)
            return;

        // Freeze physics again
        rb.isKinematic = true;
        rb.useGravity = false;

        // Reset movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Teleport back to original place
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Reset colour
        if (groundRenderer != null)
            groundRenderer.material.color = originalColor;

        // Allow triggering again
        hasTriggered = false;
    }
}

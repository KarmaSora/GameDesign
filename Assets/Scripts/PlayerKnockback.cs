using System.Collections;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackSpeed = 10f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private CharacterController controller;
    private MonoBehaviour movementScript;  // Your PlayerMovement
    private bool isKnockedBack = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Replace PlayerMovement with the actual name of your movement script
        movementScript = GetComponent<PlayerMovement>();
    }

    public void ApplyKnockback(Vector3 direction)
    {
        if (isKnockedBack) return;

        direction.y = 0f;
        direction.Normalize();

        StartCoroutine(KnockbackRoutine(direction));
    }

    private IEnumerator KnockbackRoutine(Vector3 direction)
    {
        isKnockedBack = true;

        // Disable player movement during knockback
        if (movementScript != null)
            movementScript.enabled = false;

        float timer = 0f;

        while (timer < knockbackDuration)
        {
            controller.Move(direction * knockbackSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Re-enable movement
        if (movementScript != null)
            movementScript.enabled = true;

        isKnockedBack = false;
    }
}

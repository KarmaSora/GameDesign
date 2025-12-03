using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;

    // Optional: if you still want a transform fallback (not required anymore)
    [SerializeField] private Transform target;

    [Header("Collider Positioning")]
    [Tooltip("Collider whose top will be used as the base position for the health bar.")]
    [SerializeField] private Collider targetCollider;

    [Tooltip("Extra height above the top of the collider in world units.")]
    [SerializeField] private float heightOffset = 0.5f;

    [Header("Billboarding")]
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        // Camera fallback
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // If health system not set, try to find it on parent
        if (healthSystem == null)
        {
            healthSystem = GetComponentInParent<HealthSystem>();
        }

        // Auto-find collider on the same object as the HealthSystem (or its parent)
        if (targetCollider == null && healthSystem != null)
        {
            targetCollider = healthSystem.GetComponent<Collider>();
            if (targetCollider == null)
            {
                targetCollider = healthSystem.GetComponentInChildren<Collider>();
            }
        }

        // Optional fallback: if no collider, use the health system transform
        if (target == null && healthSystem != null)
        {
            target = healthSystem.transform;
        }
    }

    private void Start()
    {
        if (healthSystem == null)
        {
            Debug.LogWarning("EnemyHealthBar: No HealthSystem reference set.", this);
            enabled = false;
            return;
        }

        if (healthSlider == null)
        {
            Debug.LogWarning("EnemyHealthBar: No Slider reference set.", this);
            enabled = false;
            return;
        }

        // Subscribe to health changes
        healthSystem.OnHealthChanged += HandleHealthChanged;

        // Initialize bar to current health
        HandleHealthChanged(healthSystem.currentHealth, healthSystem.MaxHealth);
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void LateUpdate()
    {
        // 1. Compute base position: top of collider if available
        Vector3 basePosition;

        if (targetCollider != null)
        {
            Bounds b = targetCollider.bounds;

            // Top of the collider (x,z from center, y from max)
            basePosition = new Vector3(b.center.x, b.max.y, b.center.z);
        }
        else if (target != null)
        {
            // Fallback: just use the target transform
            basePosition = target.position;
        }
        else
        {
            return; // nothing to follow
        }

        // 2. Add a small vertical offset so it floats above the collider
        basePosition += Vector3.up * heightOffset;

        // 3. Move the health bar object
        transform.position = basePosition;

        // 4. Billboard toward the camera
        if (mainCamera != null)
        {
            Vector3 dir = transform.position - mainCamera.transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (healthSlider == null || max <= 0f) return;

        float normalized = Mathf.Clamp01(current / max);
        healthSlider.value = normalized;

        // Optional: hide bar when dead
        // gameObject.SetActive(current > 0f);
    }
}

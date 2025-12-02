
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Transform target;   // usually the enemy root transform

    [Header("Positioning")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f); // height above enemy

    [Header("Billboarding")]
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        // Fallbacks to make setup easier
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (healthSystem != null && target == null)
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
        // or: HandleHealthChanged(healthSystem.CurrentHealth, healthSystem.MaxHealth);
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
        if (target == null) return;

        // Follow enemy
        transform.position = target.position + offset;

        // Face the camera (billboard)
        if (mainCamera != null)
        {
            Vector3 direction = transform.position - mainCamera.transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // This was the missing method
    private void HandleHealthChanged(float current, float max)
    {
        if (healthSlider == null || max <= 0f) return;

        float normalized = Mathf.Clamp01(current / max);
        healthSlider.value = normalized;

        // Optional: hide bar when dead
        // gameObject.SetActive(current > 0f);
    }
}

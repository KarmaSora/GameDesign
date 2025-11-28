using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] public float damage = 10f;

    // The base damage used by PlayerStats for scaling.
    // Set this in the Inspector.
    [SerializeField] private float baseDamage = 10f;

    [SerializeField] private GameObject owner;

    private void Awake()
    {
        // If baseDamage is not set in Inspector, copy starting damage.
        if (Mathf.Approximately(baseDamage, 0f))
        {
            baseDamage = damage;
        }
    }

    // Classic getter — no arrow syntax
    public float BaseDamage
    {
        get { return baseDamage; }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions with the owner
        if (owner != null && other.gameObject == owner)
        {
            return;
        }

        // Only damage enemies
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        HealthSystem healthSystem = other.GetComponent<HealthSystem>();

        if (healthSystem == null)
        {
            return;
        }

        healthSystem.TakeDamage(damage);
    }
}

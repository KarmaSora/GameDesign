using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] public float damage = 10f;

    [SerializeField] private float baseDamage = 10f;

    [SerializeField] private GameObject owner;

    private void Awake()
    {
        if (Mathf.Approximately(baseDamage, 0f))
        {
            baseDamage = damage;
        }
    }

    public float BaseDamage
    {
        get { return baseDamage; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null)
        {
            if (other.gameObject == owner)
            {
                return;
            }
        }

        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        HealthSystem health = other.GetComponent<HealthSystem>();

        if (health == null)
        {
            return;
        }

        // Damage from player's weapon
        health.TakeDamage(damage, true);
    }
}

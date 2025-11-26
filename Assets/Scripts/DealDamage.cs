using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class DealDamage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;

    [SerializeField] private GameObject owner;

    private void OnTriggerEnter(Collider other)
    {

        // Ignore collisions with the owner (if set)
        if (owner != null && other.gameObject == owner)
        {
            return;
        }
        if (!other.CompareTag("Enemy"))
        {
            return;
        }


        HealthSystem healthSystem = other.GetComponent<HealthSystem>();

        if (healthSystem == null)
        {
            // No health system found, nothing to damage
            return;
        }

        // HealthSystem will handle dying internally
        healthSystem.TakeDamage(damage);
    }


}



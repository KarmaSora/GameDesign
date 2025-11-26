using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 10f;

    // Time between hits when the player stays in the trigger
    [SerializeField] private float attackCooldown = 1.0f;

    // Tag of the player object
    [SerializeField] private string playerTag = "Player";

    private float lastAttackTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObj = other.gameObject;
        gameObj.CompareTag(playerTag);
        if (gameObj.CompareTag(playerTag))
        {

        Debug.Log("Enemy trigger ENTER with: " + other.name);
        TryDealDamage(other);

        }
    }

   
    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider other)
    {
        // Only hit the player
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        // Respect cooldown
        if (Time.time < lastAttackTime + attackCooldown)
        {
            return;
        }

        // Get HealthSystem on the player
        HealthSystem playerHealth = other.GetComponent<HealthSystem>();

        if (playerHealth == null)
        {
            Debug.LogWarning($"Object tagged {playerTag} has no HealthSystem: {other.name}");
            return;
        }

        playerHealth.TakeDamage(damage);
        lastAttackTime = Time.time;

        Debug.Log($"Enemy '{gameObject.name}' dealt {damage} damage to '{other.name}'. '{other.name}' ");
    }
}

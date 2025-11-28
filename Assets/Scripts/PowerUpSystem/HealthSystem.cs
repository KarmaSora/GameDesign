using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Classic getter for maxHealth
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    // Allows external scripts (PlayerStats, Powerups, etc.) to change max HP
    public void SetMaxHealth(float newMaxHealth, bool healToFull)
    {
        maxHealth = Mathf.Max(1f, newMaxHealth);

        if (healToFull)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        Debug.Log(gameObject.name + " new max health: " + maxHealth + ", current: " + currentHealth);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log(gameObject.name + " healed. Health: " + currentHealth + "/" + maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0.0f) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // CASE 1: Enemy -> award XP and destroy
        if (CompareTag("Enemy"))
        {
            int xpReward = 10; // default fallback

            Enemy enemyComponent = GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                xpReward = enemyComponent.XPReward;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerStats stats = player.GetComponent<PlayerStats>();
                if (stats != null)
                {
                    stats.AddXP(xpReward);
                }
                else
                {
                    Debug.LogWarning("HealthSystem.Die: Player has no PlayerStats component.");
                }
            }
            else
            {
                Debug.LogWarning("HealthSystem.Die: No 'Player' object found to award XP.");
            }

            Destroy(gameObject);
            return;
        }

        // CASE 2: Player -> use lives system instead of destroy
        if (CompareTag("Player"))
        {
            PlayerLife playerLife = GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.HandleDeath();
            }
            else
            {
                Debug.LogWarning("HealthSystem.Die: Player has no PlayerLife component.");
                // Fallback: if no PlayerLife, just destroy
                Destroy(gameObject);
            }

            return;
        }

        // CASE 3: Anything else -> just destroy
        Destroy(gameObject);
    }
}

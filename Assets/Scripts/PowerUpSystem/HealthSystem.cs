using System; // NEW: for Action<>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    // NEW: UI / other systems can subscribe to this
    // (currentHealth, maxHealth)
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;

        // NEW: tell listeners the initial values
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Classic getter for maxHealth
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    // Optional read-only helper (does not replace currentHealth!)
    public float CurrentHealth
    {
        get { return currentHealth; }
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

        // NEW: notify listeners
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log(gameObject.name + " healed. Health: " + currentHealth + "/" + maxHealth);

        // NEW: notify listeners
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0.0f) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " Health: " + currentHealth);

        // NEW: notify listeners
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // NEW: try dropping a powerup on death (for any object that has the component)
        PowerupDropOnDeath drop = GetComponent<PowerupDropOnDeath>();
        if (drop != null)
        {
            drop.DropNow();
        }

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

    // NEW: helper, nice for sliders
    public float GetHealthNormalized()
    {
        if (maxHealth <= 0f) return 0f;
        return currentHealth / maxHealth;
    }
}

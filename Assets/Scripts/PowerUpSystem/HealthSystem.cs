using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    public event Action<float, float> OnHealthChanged;

    private bool lastHitByPlayer = false;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth, maxHealth);
        }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public void SetMaxHealth(float newMaxHealth, bool healToFull)
    {
        if (newMaxHealth < 1f)
        {
            maxHealth = 1f;
        }
        else
        {
            maxHealth = newMaxHealth;
        }

        if (healToFull)
        {
            currentHealth = maxHealth;
        }
        else
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        Debug.Log(gameObject.name + " new max health: " + maxHealth + ", current: " + currentHealth);

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth, maxHealth);
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        float newHealth = currentHealth + amount;

        if (newHealth > maxHealth)
        {
            newHealth = maxHealth;
        }

        currentHealth = newHealth;

        Debug.Log(gameObject.name + " healed. Health: " + currentHealth + "/" + maxHealth);

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, false);
    }

    public void TakeDamage(float damage, bool causedByPlayer)
    {
        if (damage <= 0.0f)
        {
            return;
        }

        float oldHealth = currentHealth;

        currentHealth = currentHealth - damage;

        if (currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        lastHitByPlayer = causedByPlayer;

        float actualDamage = oldHealth - currentHealth;

        Debug.Log(gameObject.name + " took damage. Current health: " + currentHealth +
                  " | causedByPlayer = " + causedByPlayer +
                  " | actualDamage = " + actualDamage);

        // If an enemy was damaged by the player: count as damage dealt
        if (CompareTag("Enemy"))
        {
            if (causedByPlayer)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RegisterDamageDealt(actualDamage);
                }
            }
        }

        // If the player took damage: count as damage taken
        if (CompareTag("Player"))
        {
            if (actualDamage > 0f)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RegisterDamageTaken(actualDamage);
                }
            }
        }

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth, maxHealth);
        }

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        PowerupDropOnDeath drop = GetComponent<PowerupDropOnDeath>();
        if (drop != null)
        {
            drop.DropNow();
        }

        // ENEMY DEATH
        if (CompareTag("Enemy"))
        {
            if (lastHitByPlayer)
            {
                int xpReward = 10;

                Enemy enemyComponent = GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    xpReward = enemyComponent.XPReward;
                }

                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    PlayerStats stats = playerObject.GetComponent<PlayerStats>();
                    if (stats != null)
                    {
                        stats.AddXP(xpReward);
                    }
                    else
                    {
                        Debug.LogWarning("HealthSystem.Die: PlayerStats missing on Player.");
                    }
                }
                else
                {
                    Debug.LogWarning("HealthSystem.Die: No Player object found.");
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RegisterEnemyKill();
                }
            }
            else
            {
                Debug.Log("Enemy died without player hit. No XP or kill awarded.");
            }

            Destroy(gameObject);
            return;
        }

        // PLAYER DEATH
        if (CompareTag("Player"))
        {
            PlayerLife life = GetComponent<PlayerLife>();

            if (life != null)
            {
                life.HandleDeath();
            }
            else
            {
                Debug.LogWarning("PlayerLife component missing. Destroying player.");
                Destroy(gameObject);
            }

            return;
        }

        // ANY OTHER OBJECT
        Destroy(gameObject);
    }

    public float GetHealthNormalized()
    {
        if (maxHealth <= 0f)
        {
            return 0f;
        }

        float result = currentHealth / maxHealth;
        return result;
    }
}

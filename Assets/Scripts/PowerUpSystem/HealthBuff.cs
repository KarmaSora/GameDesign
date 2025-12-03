using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Healthbuff")]
public class HealthBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("[HealthBuff] Target is null, cannot apply buff.");
            return;
        }

        HealthSystem healthSystem = target.GetComponent<HealthSystem>();

        if (healthSystem == null)
        {
            Debug.LogWarning("[HealthBuff] No HealthSystem found on target: " + target.name);
            return;
        }

        float beforeHealth = healthSystem.currentHealth;

        // Apply heal
        healthSystem.currentHealth += amount;

        // Clamp to maxHealth if that is how your system is intended to work
        if (healthSystem.currentHealth > healthSystem.MaxHealth)
        {
            healthSystem.currentHealth = healthSystem.MaxHealth;
        }

        if (healthSystem.currentHealth < 0f)
        {
            healthSystem.currentHealth = 0f;
        }

        Debug.Log(
            "[HealthBuff] Activated on " + target.name +
            " | before = " + beforeHealth +
            " | amount = " + amount +
            " | after = " + healthSystem.currentHealth
        );
    }
}

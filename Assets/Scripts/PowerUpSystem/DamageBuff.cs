using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Damagebuff")]
public class DamageBuff : PowerupEffect
{
    // How much extra damage this buff should add
    public float amount;

    public override void Apply(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("[DamageBuff] Target is null, cannot apply buff.");
            return;
        }

        // Try to get the component that actually holds the damage value
        DealDamage dealDamage = target.GetComponent<DealDamage>();

        if (dealDamage == null)
        {
            dealDamage = target.GetComponentInChildren<DealDamage>();

            Debug.LogWarning("[DamageBuff] No DealDamage component found on target: " + target.name);
            return;
        }
        if (dealDamage == null)
        {

            Debug.LogWarning("[DamageBuff] No DealDamage component found on target: " + target.name);
            return;
        }

        // Store the old value so we can see what changed
        float oldDamage = dealDamage.damage;

        // Apply a flat increase
        dealDamage.damage += amount;

        // Safety: never let damage go below zero (in case amount is negative)
        if (dealDamage.damage < 0f)
        {
            dealDamage.damage = 0f;
        }

        Debug.Log("[DamageBuff] Applied to " + target.name +
                  " | oldDamage = " + oldDamage +
                  " | amount = " + amount +
                  " | newDamage = " + dealDamage.damage);
    }
}

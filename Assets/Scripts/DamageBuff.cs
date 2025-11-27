using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/DamageBuff")]
public class DamageBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        string weaponTag = "Weapon";
        string playerTag = "Player";

        // CASE 1: Target is directly the weapon
        if (target.CompareTag(weaponTag))
        {
            DealDamage dmg = target.GetComponent<DealDamage>();

            if (dmg != null)
            {
                dmg.damage += amount;
                Debug.Log("DamageBuff applied directly to weapon: +" + amount);
            }
            else
            {
                Debug.LogWarning("Weapon does not have DealDamage component!");
            }
            return;
        }

        // CASE 2: Target is the player -> find weapon in children
        if (target.CompareTag(playerTag))
        {
            Transform[] allChildren = target.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.CompareTag(weaponTag))
                {
                    DealDamage dmg = child.GetComponent<DealDamage>();
                    if (dmg != null)
                    {
                        dmg.damage += amount;
                        Debug.Log("DamageBuff applied to player’s weapon: +" + amount);
                    }
                    else
                    {
                        Debug.LogWarning("Weapon found but missing DealDamage component!");
                    }

                    return; // Stop after first weapon
                }
            }

            Debug.LogWarning("Player has no weapon tagged 'Weapon' in hierarchy.");
        }
    }
}

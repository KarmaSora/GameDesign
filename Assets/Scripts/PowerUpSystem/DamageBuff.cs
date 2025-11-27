using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/DamageBuff")]
public class DamageBuff : PowerupEffect
{
    [Header("Damage Buff Settings")]
    public float amount = 3f;     // How much extra damage
    public float duration = 3f;   // How long the buff lasts

    private const string weaponTag = "Weapon";
    private const string playerTag = "Player";

    public override void Apply(GameObject target)
    {
        GameObject weaponGO = null;

        // CASE 1: Target IS the weapon
        if (target.CompareTag(weaponTag))
        {
            weaponGO = target;
        }
        // CASE 2: Target is the player -> search children for weapon
        else if (target.CompareTag(playerTag))
        {
            Transform[] allChildren = target.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.CompareTag(weaponTag))
                {
                    weaponGO = child.gameObject;
                    break; // Stop after first weapon found
                }
            }
        }

        // If we didn't find a weapon, log a warning and stop
        if (weaponGO == null)
        {
            Debug.LogWarning("DamageBuff: No GameObject with tag 'Weapon' found on target.");
            return;
        }

        // Get DealDamage component on the weapon
        DealDamage dmg = weaponGO.GetComponent<DealDamage>();
        if (dmg == null)
        {
            Debug.LogWarning("DamageBuff: Weapon found, but it has no DealDamage component!");
            return;
        }

        // Start coroutine on the DealDamage component (it’s a MonoBehaviour)
        dmg.StartCoroutine(ApplyDamageBuffCoroutine(dmg));
    }

    private IEnumerator ApplyDamageBuffCoroutine(DealDamage dmg)
    {
        // 1. Apply buff
        dmg.damage += amount;
        Debug.Log("DamageBuff activated: +" + amount + " damage for " + duration + " seconds.");

        // 2. Wait
        yield return new WaitForSeconds(duration);

        // 3. Revert buff
        dmg.damage -= amount;
        Debug.Log("DamageBuff expired: -" + amount + " damage.");
    }
}

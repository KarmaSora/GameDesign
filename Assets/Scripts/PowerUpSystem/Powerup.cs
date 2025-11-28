using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Only the player should pick up powerups
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (powerupEffect == null)
        {
            Debug.LogWarning("Powerup: powerupEffect is not assigned on " + gameObject.name);
            return;
        }

        // Apply effect to the player
        powerupEffect.Apply(other.gameObject);

        // Then destroy the pickup
        Destroy(gameObject);
    }
}

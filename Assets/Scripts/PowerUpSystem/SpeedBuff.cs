using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Powerups/SpeedBuff")]
public class SpeedBuff : PowerupEffect
{
    [Header("Speed Buff Settings")]
    public float amount = 5f;      // How much to add
    public float duration = 5f;    // How many seconds it lasts

    public override void Apply(GameObject target)
    {
        // Get the movement component on the player
        PlayerMovement movement = target.GetComponent<PlayerMovement>();

        if (movement == null)
        {
            Debug.LogWarning("SpeedBuff: target has no PlayerMovement component.");
            return;
        }

        // Start a coroutine on the player to handle the timed buff
        movement.StartCoroutine(ApplySpeedBuffCoroutine(movement));
    }

    private IEnumerator ApplySpeedBuffCoroutine(PlayerMovement movement)
    {
        // 1. Apply the buff
        movement.moveSpeedIncreaser += amount;
        Debug.Log("SpeedBuff activated: +" + amount + " for " + duration + " seconds.");

        // 2. Wait for 'duration' seconds (this is the "temporary" part)
        yield return new WaitForSeconds(duration);

        // 3. Remove the buff again
        movement.moveSpeedIncreaser -= amount;
        Debug.Log("SpeedBuff expired: -" + amount);
    }
}

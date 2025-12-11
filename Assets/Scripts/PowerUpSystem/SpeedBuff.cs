using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : PowerupEffect
{
    [Header("Speed Buff Settings")]
    public float amount = 5f;      // How much to add
    public float duration = 5f;    // How many seconds it lasts

    public override void Apply(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("SpeedBuff: Target is null.");
            return;
        }

        PlayerMovement movement = target.GetComponent<PlayerMovement>();

        if (movement == null)
        {
            Debug.LogWarning("SpeedBuff: target has no PlayerMovement component.");
            return;
        }

        // Show indicator on player
        PowerupIndicatorController indicator = target.GetComponent<PowerupIndicatorController>();

        if (indicator != null)
        {
            indicator.ShowIndicator(PowerupVisualType.Speed, duration);
        }

        // Start a coroutine on the player to handle the timed buff
        movement.StartCoroutine(ApplySpeedBuffCoroutine(movement));
    }

    private IEnumerator ApplySpeedBuffCoroutine(PlayerMovement movement)
    {
        movement.moveSpeedIncreaser += amount;
        Debug.Log("SpeedBuff activated: +" + amount + " for " + duration + " seconds.");

        yield return new WaitForSeconds(duration);

        movement.moveSpeedIncreaser -= amount;
        Debug.Log("SpeedBuff expired: -" + amount);
    }
}

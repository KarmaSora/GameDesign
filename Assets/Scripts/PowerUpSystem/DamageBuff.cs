using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Damagebuff")]
public class DamageBuff : PowerupEffect
{
    [Header("Damage Buff Settings")]
    public float amount = 10f;      // Extra damage on top of base
    public float duration = 5f;     // How long the buff lasts

    [Header("Indicator Settings")]
    public float indicatorDurationOverride = -1f;

    public override void Apply(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("[DamageBuff] Target is null, cannot apply buff.");
            return;
        }

        DealDamage dealDamage = target.GetComponentInChildren<DealDamage>();

        if (dealDamage == null)
        {
            Debug.LogWarning("[DamageBuff] No DealDamage component found on target or children: " + target.name);
            return;
        }

        PowerupIndicatorController indicator = target.GetComponent<PowerupIndicatorController>();

        if (indicator != null)
        {
            float indicatorDuration = duration;

            if (indicatorDurationOverride > 0f)
            {
                indicatorDuration = indicatorDurationOverride;
            }

            indicator.ShowIndicator(PowerupVisualType.Damage, indicatorDuration);
        }

        PlayerMovement runner = target.GetComponent<PlayerMovement>();

        if (runner == null)
        {
            Debug.LogWarning("[DamageBuff] No PlayerMovement found on target to run coroutine: " + target.name);
            return;
        }

        runner.StartCoroutine(ApplyDamageBuffCoroutine(dealDamage));
    }

    private IEnumerator ApplyDamageBuffCoroutine(DealDamage dealDamage)
    {
        // Add temporary bonus
        dealDamage.AddDamageBuffBonus(amount);

        Debug.Log(
            "[DamageBuff] Activated. Added buff amount: " + amount +
            " | total damage now = " + dealDamage.CurrentDamage +
            " | duration = " + duration
        );

        yield return new WaitForSeconds(duration);

        // Remove the same bonus
        dealDamage.AddDamageBuffBonus(-amount);

        Debug.Log(
            "[DamageBuff] Expired. Removed buff amount: " + amount +
            " | total damage now = " + dealDamage.CurrentDamage
        );
    }
}

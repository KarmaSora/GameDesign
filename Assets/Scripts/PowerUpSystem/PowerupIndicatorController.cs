using System.Collections;
using UnityEngine;

public enum PowerupVisualType
{
    Speed,
    Jump,
    Damage,
    Health
}


public class PowerupIndicatorController : MonoBehaviour
{
    [Header("Parent / container for indicators")]
    [SerializeField] private Transform indicatorParent;   // e.g. your "PowerupIndicator" object
    [SerializeField] private Vector3 indicatorOffset = new Vector3(0f, -0.4f, 0f);

    [Header("Per-powerup indicator objects")]
    [SerializeField] private GameObject speedIndicator;
    [SerializeField] private GameObject jumpIndicator;
    [SerializeField] private GameObject damageIndicator;
    [SerializeField] private GameObject healthIndicator;

    // One coroutine handle per type so they can overlap
    private Coroutine speedRoutine;
    private Coroutine jumpRoutine;
    private Coroutine damageRoutine;
    private Coroutine healthRoutine;

    private void Start()
    {
        SetIndicatorActive(speedIndicator, false);
        SetIndicatorActive(jumpIndicator, false);
        SetIndicatorActive(damageIndicator, false);
        SetIndicatorActive(healthIndicator, false);
    }

    private void Update()
    {
        // Optional: keep a parent at player position + offset (Unity tutorial style)
        if (indicatorParent == null)
        {
            return;
        }

        indicatorParent.position = transform.position + indicatorOffset;
    }

    /// <summary>
    /// Called by powerups to show a visual indicator for a certain duration.
    /// </summary>
    public void ShowIndicator(PowerupVisualType type, float duration)
    {
        if (duration <= 0f)
        {
            return;
        }

        switch (type)
        {
            case PowerupVisualType.Speed:
                RestartIndicator(ref speedRoutine, speedIndicator, duration);
                break;

            case PowerupVisualType.Jump:
                RestartIndicator(ref jumpRoutine, jumpIndicator, duration);
                break;

            case PowerupVisualType.Damage:
                RestartIndicator(ref damageRoutine, damageIndicator, duration);
                break;

            case PowerupVisualType.Health:
                RestartIndicator(ref healthRoutine, healthIndicator, duration);
                break;
        }
    }

    private void RestartIndicator(ref Coroutine routineField, GameObject indicator, float duration)
    {
        if (indicator == null)
        {
            return;
        }

        if (routineField != null)
        {
            StopCoroutine(routineField);
        }

        routineField = StartCoroutine(IndicatorRoutine(indicator, duration));
    }

    private IEnumerator IndicatorRoutine(GameObject indicator, float duration)
    {
        SetIndicatorActive(indicator, true);
        yield return new WaitForSeconds(duration);
        SetIndicatorActive(indicator, false);
    }

    private void SetIndicatorActive(GameObject indicator, bool state)
    {
        if (indicator == null)
        {
            return;
        }

        indicator.SetActive(state);
    }
}

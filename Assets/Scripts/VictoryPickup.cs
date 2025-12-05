using UnityEngine;

public class VictoryPickup : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.LogWarning("VictoryPickup: GameManager.Instance is null. Make sure there is a GameManager in the scene.");
        }

        Destroy(gameObject);
    }
}

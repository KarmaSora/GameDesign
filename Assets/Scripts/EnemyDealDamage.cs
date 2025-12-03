using System.Collections;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 10f;

    [Tooltip("Time between finished attacks.")]
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Attack Telegraph")]
    [Tooltip("How long the enemy warns before dealing damage.")]
    [SerializeField] private float windupTime = 0.4f;

    [Tooltip("How far from this object the hit is allowed to connect.")]
    [SerializeField] private float hitRange = 2.0f;

    [Tooltip("Renderer that will change color during windup.")]
    [SerializeField] private Renderer enemyRenderer;

    [SerializeField] private Color idleColor = Color.white;
    [SerializeField] private Color windupColor = Color.red;

    [Header("Player Detection")]
    [SerializeField] private string playerTag = "Player";

    private float lastAttackTime = -999f;
    private bool isAttacking = false;

    private void Awake()
    {
        // Try to auto find a Renderer if none has been assigned
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponent<Renderer>();
        }

        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInChildren<Renderer>();
        }

        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInParent<Renderer>();
        }

        if (enemyRenderer == null)
        {
            Debug.LogWarning("EnemyDealDamage: No Renderer found for telegraph on " + gameObject.name);
        }
        else
        {
            // Make sure we start with idle color
            enemyRenderer.material.color = idleColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryStartAttack(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryStartAttack(other);
    }

    private void TryStartAttack(Collider other)
    {
        // Only care about the player
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        // Already doing an attack right now
        if (isAttacking)
        {
            return;
        }

        // Respect cooldown
        if (Time.time < lastAttackTime + attackCooldown)
        {
            return;
        }

        // Start one attack sequence
        StartCoroutine(AttackRoutine(other));
    }

    private IEnumerator AttackRoutine(Collider other)
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // 1. Telegraph phase: change color
        SetTelegraphColor(true);

        float timer = 0f;
        while (timer < windupTime)
        {
            // If the player left during windup, cancel the attack
            if (other == null || !other.CompareTag(playerTag))
            {
                SetTelegraphColor(false);
                isAttacking = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // End telegraph color
        SetTelegraphColor(false);

        // 2. Attack phase
        if (other == null || !other.CompareTag(playerTag))
        {
            isAttacking = false;
            yield break;
        }

        float distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance > hitRange)
        {
            // Player dodged
            isAttacking = false;
            yield break;
        }

        // Check block
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.IsBlocking)
        {
            Debug.Log("Enemy attack was blocked by the player.");
            isAttacking = false;
            yield break;
        }

        // Deal damage
        HealthSystem playerHealth = other.GetComponent<HealthSystem>();
        if (playerHealth == null)
        {
            Debug.LogWarning("EnemyDealDamage: Player has no HealthSystem component.");
            isAttacking = false;
            yield break;
        }

        playerHealth.TakeDamage(damage);
        Debug.Log("Enemy '" + gameObject.name + "' dealt " + damage + " damage to '" + other.name + "'.");

        isAttacking = false;
    }

    private void SetTelegraphColor(bool isWindup)
    {
        if (enemyRenderer == null)
        {
            return;
        }

        if (isWindup)
        {
            enemyRenderer.material.color = windupColor;
        }
        else
        {
            enemyRenderer.material.color = idleColor;
        }
    }
}

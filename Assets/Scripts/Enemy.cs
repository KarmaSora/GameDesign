using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float enemySpeed = 3.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f;

    [Header("Area Limits")]
    [SerializeField] private bool limitToHomeRadius = true;
    [SerializeField] private float homeRadius = 12f;

    [Header("XP Settings")]
    [SerializeField] private int xpReward = 25;

    [Header("References")]
    [SerializeField] private Rigidbody enemyRB;
    [SerializeField] private Transform player;

    private Vector3 homePosition;

    public int XPReward
    {
        get { return xpReward; }
    }

    private void Awake()
    {
        if (enemyRB == null)
        {
            enemyRB = GetComponent<Rigidbody>();
        }

        if (enemyRB == null)
        {
            Debug.LogError("Enemy: No Rigidbody found on this GameObject.");
        }
        else
        {
            enemyRB.freezeRotation = false;
            enemyRB.constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        homePosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (enemyRB == null || player == null)
            return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        float sqrDistanceToPlayer = toPlayer.sqrMagnitude;
        float sqrDetectionRadius = detectionRadius * detectionRadius;

        if (sqrDistanceToPlayer > sqrDetectionRadius)
        {
            enemyRB.velocity = Vector3.zero;
            return;
        }

        if (sqrDistanceToPlayer < 0.0001f)
            return;

        Vector3 direction = toPlayer.normalized;

        Vector3 targetPosition = enemyRB.position + direction * enemySpeed * Time.fixedDeltaTime;

        if (limitToHomeRadius)
        {
            Vector3 offsetFromHome = targetPosition - homePosition;
            offsetFromHome.y = 0f;

            float sqrHomeRadius = homeRadius * homeRadius;

            if (offsetFromHome.sqrMagnitude > sqrHomeRadius)
            {
                offsetFromHome = offsetFromHome.normalized * homeRadius;

                targetPosition = new Vector3(
                    homePosition.x + offsetFromHome.x,
                    targetPosition.y,
                    homePosition.z + offsetFromHome.z
                );
            }
        }

        enemyRB.MovePosition(targetPosition);

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion newRotation = Quaternion.Slerp(
            enemyRB.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
        enemyRB.MoveRotation(newRotation);

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (limitToHomeRadius)
        {
            Gizmos.color = Color.blue;
            Vector3 center = Application.isPlaying ? homePosition : transform.position;
            Gizmos.DrawWireSphere(center, homeRadius);
        }
    }
}

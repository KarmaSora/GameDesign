using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float enemySpeed = 3.0f;      // meters per second
    [SerializeField] private float rotationSpeed = 5.0f;   // how fast to turn toward player

    [Header("References")]
    [SerializeField] private Rigidbody enemyRB;
    [SerializeField] private Transform player;

    private void Awake()
    {
        // Make sure we have a Rigidbody
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
            // Prevent tipping over: only allow rotation around Y
            enemyRB.freezeRotation = false;
            enemyRB.constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void Start()
    {
        // Find the player by tag (recommended):
        // Make sure your player GameObject has the tag "Player"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Enemy: No GameObject with tag 'Player' found in the scene.");
            }
        }
    }

    private void FixedUpdate()
    {
        if (enemyRB == null || player == null)
            return;

        // 1. Calculate direction on the XZ-plane only (ignore height difference)
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f; // keep movement flat on the ground

        if (toPlayer.sqrMagnitude < 0.0001f)
            return; // already on top of the player

        Vector3 direction = toPlayer.normalized;

        // 2. Move towards the player with constant speed
        Vector3 targetPosition = enemyRB.position + direction * enemySpeed * Time.fixedDeltaTime;
        enemyRB.MovePosition(targetPosition);

        // 3. Rotate to face the player smoothly
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion newRotation = Quaternion.Slerp(enemyRB.rotation, targetRotation,
                                                  rotationSpeed * Time.fixedDeltaTime);
        enemyRB.MoveRotation(newRotation);

        // 4. Optional: destroy if it falls off the world
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}

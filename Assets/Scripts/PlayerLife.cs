using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Lives Settings")]
    [SerializeField] private int startingLives = 3;

    // If the player falls below this Y value, they lose a life and respawn
    [SerializeField] private float fallThreshold = -10f;

    private int currentLives;
    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

    private HealthSystem healthSystem;
    private CharacterController characterController;

    // To prevent multiple deaths being processed at once
    private bool isProcessingDeath = false;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Set initial lives and store the starting position/rotation as respawn point
        currentLives = startingLives;
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
    }

    private void Update()
    {
        // FALL DEATH CHECK
        if (transform.position.y < fallThreshold)
        {
            HandleDeath();
        }
    }

    // Called by HealthSystem when the player "dies" from damage
    public void HandleDeath()
    {
        if (isProcessingDeath)
        {
            // Already handling a death (fall or HP), ignore extra calls
            return;
        }

        isProcessingDeath = true;

        currentLives--;

        Debug.Log("Player died. Lives left: " + currentLives);

        if (currentLives > 0)
        {
            Respawn();
            isProcessingDeath = false;  // Ready to detect future deaths
        }
        else
        {
            Debug.Log("Game Over. No lives remaining.");
            // Here you can instead trigger a Game Over screen or reload the scene.
            Destroy(gameObject);
        }
    }

    private void Respawn()
    {
        // Restore health to full
        if (healthSystem != null)
        {
            float max = healthSystem.MaxHealth;
            healthSystem.SetMaxHealth(max, true);
        }

        // Teleport player back to respawn point
        if (characterController != null)
        {
            characterController.enabled = false;
            transform.position = respawnPosition;
            transform.rotation = respawnRotation;
            characterController.enabled = true;
        }
        else
        {
            transform.position = respawnPosition;
            transform.rotation = respawnRotation;
        }

        Debug.Log("Player respawned at starting position.");
    }

    public int CurrentLives
    {
        get { return currentLives; }
    }

    public int StartingLives
    {
        get { return startingLives; }
    }
}

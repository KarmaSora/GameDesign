using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Player References")]
    [SerializeField] private PlayerMovement playerMovement;

    private bool isGameStarted = false;
    private bool isGameOver = false;

    private void Awake()
    {
        // Simple singleton pattern so we can call GameManager.Instance from other scripts
        if (Instance == null)
        {
            Instance = this;
            // Optionally keep this object across scenes:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ShowStartMenu();
    }

    private void Update()
    {
        // Start game when SPACE is pressed
        if (!isGameStarted && !isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // If game is over, allow restart with R
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void ShowStartMenu()
    {
        isGameStarted = false;
        isGameOver = false;

        // Pause game so nothing moves
        Time.timeScale = 0f;

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Disable player movement at start
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    private void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;

        // Resume time
        Time.timeScale = 1f;

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        // Enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // Pause game
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Stop player from moving
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // You could also disable enemy AI here if needed
    }

    private void RestartGame()
    {
        // Make sure timeScale is normal again
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}

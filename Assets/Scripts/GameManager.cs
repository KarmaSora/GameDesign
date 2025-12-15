using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [Header("Player Movement Reference")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Player Stats References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private DealDamage playerWeapon;
    [SerializeField] private HealthSystem playerHealth;
    [SerializeField] private PlayerLife playerLife;

    [Header("End Screen Stats UI")]
    [SerializeField] private TextMeshProUGUI winStatsText;
    [SerializeField] private TextMeshProUGUI gameOverStatsText;

    private bool isGameStarted = false;
    private bool isGameOver = false;
    private bool isGamePaused = false;

    // Stats
    private int enemiesKilled = 0;
    private float totalDamageDealt = 0f;
    private float totalDamageTaken = 0f;




    [SerializeField] private BackgroundMusic backgroundMusic;

    [SerializeField] private int uiMusicIndex = 0;
    [SerializeField] private int gameplayMusicIndex = 1;


    public int EnemiesKilled
    {
        get { return enemiesKilled; }
    }

    public float TotalDamageDealt
    {
        get { return totalDamageDealt; }
    }

    public float TotalDamageTaken
    {
        get { return totalDamageTaken; }
    }
    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (BackgroundMusic.Instance != null)
        {
            backgroundMusic = BackgroundMusic.Instance;
        }



        // Try to auto-find player and its components if not assigned
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            if (playerMovement == null)
            {
                playerMovement = playerObject.GetComponent<PlayerMovement>();
            }

            if (playerStats == null)
            {
                playerStats = playerObject.GetComponent<PlayerStats>();
            }

            if (playerHealth == null)
            {
                playerHealth = playerObject.GetComponent<HealthSystem>();
            }

            if (playerLife == null)
            {
                playerLife = playerObject.GetComponent<PlayerLife>();
            }

            if (playerWeapon == null && playerStats != null)
            {
                Transform[] children = playerStats.GetComponentsInChildren<Transform>(true);

                for (int i = 0; i < children.Length; i++)
                {
                    Transform t = children[i];

                    if (t.CompareTag("Weapon"))
                    {
                        playerWeapon = t.GetComponent<DealDamage>();

                        if (playerWeapon != null)
                        {
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("GameManager: No object with tag 'Player' found in scene.");
        }
    }

    private void Start()
    {
        ShowStartMenu();
    }

    private void Update()
    {
        // 1) If game is over, only allow restart
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();

            }

            return;
        }

        // 2) Before game has started: Space starts the game
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();

            }

            return;
        }

        // 3) Game has started and is not over

        // If currently paused: Space resumes
        if (isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResumeGame();

            }

            return;
        }

        // If running: Escape pauses
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();

        }
    }


    private void ShowStartMenu()
    {
        isGameStarted = false;
        isGameOver = false;
        isGamePaused = false;

        Time.timeScale = 0f;

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Reset stats here if you restart to main menu at some point
        enemiesKilled = 0;
        totalDamageDealt = 0f;
        totalDamageTaken = 0f;
        PlayUIMusic();

    }

    private void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;
        isGamePaused = false;

        Time.timeScale = 1f;

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // Reset stats at the beginning of a run
        enemiesKilled = 0;
        totalDamageDealt = 0f;
        totalDamageTaken = 0f;
        PlayGameplayMusic();

    }

    public void GameOver()
    {
        isGameOver = true;
        isGamePaused = false;

        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        UpdateEndScreenStats(gameOverStatsText);
        PlayUIMusic();

    }

    public void WinGame()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        isGamePaused = false;

        Time.timeScale = 0f;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        UpdateEndScreenStats(winStatsText);
        PlayUIMusic();

    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Called from HealthSystem when an enemy is killed by the player
    public void RegisterEnemyKill()
    {
        enemiesKilled = enemiesKilled + 1;
        Debug.Log("GameManager: Enemies killed = " + enemiesKilled);
    }

    // Called from HealthSystem when player deals damage to an enemy
    public void RegisterDamageDealt(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        totalDamageDealt = totalDamageDealt + amount;
    }

    // Called from HealthSystem when the player takes damage
    public void RegisterDamageTaken(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        totalDamageTaken = totalDamageTaken + amount;
    }

    private void UpdateEndScreenStats(TextMeshProUGUI targetText)
    {
        if (targetText == null)
        {
            Debug.LogError("GameManager.UpdateEndScreenStats: targetText is NULL. Did you assign the Win/GameOver Stats Text in the Inspector?");

            return;
        }

        int level = 0;
        int currentXP = 0;
        int xpToNextLevel = 0;

        if (playerStats != null)
        {
            level = playerStats.Level;
            currentXP = playerStats.CurrentXP;
            xpToNextLevel = playerStats.XPToNextLevel;
        }

        float damageStat = 0f;

        if (playerWeapon != null)
        {
            damageStat = playerWeapon.damage;
        }

        float currentHealth = 0f;
        float maxHealth = 0f;

        if (playerHealth != null)
        {
            currentHealth = playerHealth.currentHealth;
            maxHealth = playerHealth.MaxHealth;
        }

        int lives = 0;

        if (playerLife != null)
        {
            lives = playerLife.CurrentLives;
        }

        string statsText = "";
        statsText += "Level: " + level + "\n";
        statsText += "XP: " + currentXP + " / " + xpToNextLevel + "\n";
        statsText += "Damage (weapon): " + damageStat.ToString("0") + "\n";
        statsText += "Health: " + currentHealth.ToString("0") + " / " + maxHealth.ToString("0") + "\n";
        statsText += "Lives left: " + lives + "\n";
        statsText += "Enemies defeated: " + enemiesKilled + "\n";
        statsText += "Total damage dealt: " + totalDamageDealt.ToString("0") + "\n";
        statsText += "Total damage taken: " + totalDamageTaken.ToString("0") + "\n";

        targetText.text = statsText;
    }

    private void PauseGame()
    {
        // Do not pause if game has not started or is already over
        if (!isGameStarted || isGameOver)
        {
            return;
        }

        if (isGamePaused)
        {
            return;
        }

        isGamePaused = true;
        Time.timeScale = 0f;

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        PlayUIMusic();
    }

    private void ResumeGame()
    {
        if (!isGamePaused || isGameOver)
        {
            return;
        }

        isGamePaused = false;
        Time.timeScale = 1f;

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        PlayGameplayMusic();
    }



    private void PlayUIMusic()
    {
        if (backgroundMusic == null)
        {
            if (BackgroundMusic.Instance != null)
            {
                backgroundMusic = BackgroundMusic.Instance;
            }
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.SelectIndex(uiMusicIndex, false);
        }
    }

    private void PlayGameplayMusic()
    {
        if (backgroundMusic == null)
        {
            if (BackgroundMusic.Instance != null)
            {
                backgroundMusic = BackgroundMusic.Instance;
            }
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.SelectIndex(gameplayMusicIndex, false);
        }
    }



}

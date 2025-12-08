using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private DealDamage weaponDamage;
    [SerializeField] private HealthSystem playerHealth;
    [SerializeField] private PlayerLife playerLife;

    [Header("XP UI")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;

    [Header("Damage UI")]
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Lives UI")]
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Kills UI")]
    [SerializeField] private TextMeshProUGUI killsText;

    private void Awake()
    {
        // Auto-find references if not assigned
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (playerStats == null)
            {
                playerStats = player.GetComponent<PlayerStats>();
            }
            if (playerHealth == null)
            {
                playerHealth = player.GetComponent<HealthSystem>();
            }
            if (playerLife == null)
            {
                playerLife = player.GetComponent<PlayerLife>();
            }

            if (weaponDamage == null && playerStats != null)
            {
                Transform[] children = playerStats.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Weapon"))
                    {
                        weaponDamage = child.GetComponent<DealDamage>();
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerUI: No GameObject with tag 'Player' found in the scene.");
        }

        if (xpSlider == null)
        {
            Debug.LogWarning("PlayerUI: XP Slider is not assigned in the Inspector.");
        }

        if (healthSlider == null)
        {
            Debug.LogWarning("PlayerUI: Health Slider is not assigned in the Inspector.");
        }

        if (levelText == null || xpText == null || damageText == null || healthText == null || livesText == null)
        {
            Debug.LogWarning("PlayerUI: One or more Text fields are not assigned in the Inspector.");
        }
    }

    private void Start()
    {
        RefreshAll();
    }

    private void Update()
    {
        RefreshXPUI();
        RefreshDamageUI();
        RefreshHealthUI();
        RefreshLivesUI();
        RefreshKillsUI();
    }

    private void RefreshAll()
    {
        RefreshXPUI();
        RefreshDamageUI();
        RefreshHealthUI();
        RefreshLivesUI();
        RefreshKillsUI();
    }

    private void RefreshXPUI()
    {
        if (playerStats == null || xpSlider == null)
        {
            return;
        }

        int currentXP = playerStats.CurrentXP;
        int xpToNext = playerStats.XPToNextLevel;

        float ratio = 0f;
        if (xpToNext > 0)
        {
            ratio = (float)currentXP / (float)xpToNext;
        }

        xpSlider.value = ratio;

        if (levelText != null)
        {
            levelText.text = "Lv " + playerStats.Level;
        }

        if (xpText != null)
        {
            xpText.text = currentXP + " / " + xpToNext + " XP";
        }
    }

    private void RefreshDamageUI()
    {
        if (damageText == null)
        {
            return;
        }

        if (weaponDamage != null)
        {
            float dmg = weaponDamage.damage;
            damageText.text = "DMG: " + dmg.ToString("0");
        }
        else
        {
            damageText.text = "DMG: ?";
        }
    }

    private void RefreshHealthUI()
    {
        if (playerHealth == null || healthSlider == null)
        {
            return;
        }

        float current = playerHealth.currentHealth;
        float max = playerHealth.MaxHealth;

        float ratio = 0f;
        if (max > 0f)
        {
            ratio = current / max;
        }

        healthSlider.value = ratio;

        if (healthText != null)
        {
            healthText.text = current.ToString("0") + " / " + max.ToString("0");
        }
    }

    private void RefreshLivesUI()
    {
        if (livesText == null || playerLife == null)
        {
            return;
        }

        int lives = playerLife.CurrentLives;
        livesText.text = "Lives: " + lives;
    }

    private void RefreshKillsUI()
    {
        if (killsText == null)
            return;

        if (GameManager.Instance != null)
        {
            killsText.text = "Kills: " + GameManager.Instance.EnemiesKilled;
        }
        else
        {
            killsText.text = "Kills: 0";
        }
    }
}

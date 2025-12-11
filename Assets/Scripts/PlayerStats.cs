using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;
    [SerializeField] private float xpGrowthFactor = 1.5f;

    [Header("Damage Scaling")]
    [SerializeField] private float damageBonusPerLevel = 5f;

    [Header("Health Scaling")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float healthPerLevel = 12f;

    private HealthSystem healthSystem;
    private DealDamage weaponDamage;


    private float baseWeaponDamageAtLevel1;


    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform t in children)
        {
            if (t.CompareTag("Weapon"))
            {
                weaponDamage = t.GetComponent<DealDamage>();
                break;
            }
        }
        if (weaponDamage != null)
        {
            baseWeaponDamageAtLevel1 = weaponDamage.BaseDamage;
        }

    }

    private void Start()
    {
        ApplyLevelStats();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthFactor);

        ApplyLevelStats();
    }

    private void ApplyLevelStats()
    {
        if (healthSystem != null)
        {
            float newMaxHealth = baseMaxHealth + (level - 1) * healthPerLevel;
            healthSystem.SetMaxHealth(newMaxHealth, true);
        }

        if (weaponDamage != null)
        {
     
            float scaledBaseDamage = baseWeaponDamageAtLevel1 + (level - 1) * damageBonusPerLevel;


            weaponDamage.SetBaseDamage(scaledBaseDamage);

        }
    }

    // CLASSIC GETTERS (NO => syntax)

    public int Level
    {
        get { return level; }
    }

    public int CurrentXP
    {
        get { return currentXP; }
    }

    public int XPToNextLevel
    {
        get { return xpToNextLevel; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] public float damage = 10f;

    [SerializeField] private float baseDamage = 10f;

    [SerializeField] private GameObject owner;
    [SerializeField] private float damageBuffBonus = 0f;

    private void Awake()
    {
        if (Mathf.Approximately(baseDamage, 0f))
        {
            baseDamage = damage;
        }
        UpdateEffectiveDamage();

    }

    public float BaseDamage
    {
        get { return baseDamage; }
    }
    public float CurrentDamage
    {
        get { return damage; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null)
        {
            if (other.gameObject == owner)
            {
                return;
            }
        }

        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        HealthSystem health = other.GetComponent<HealthSystem>();

        if (health == null)
        {
            return;
        }

        // Damage from player's weapon
        health.TakeDamage(damage, true);
    }
    public void SetBaseDamage(float newBaseDamage)
    {
        baseDamage = newBaseDamage;
        UpdateEffectiveDamage();
    }
    public void AddDamageBuffBonus(float amount)
    {
        damageBuffBonus = damageBuffBonus + amount;
        UpdateEffectiveDamage();
    }
    public void ClearDamageBuffBonus()
    {
        damageBuffBonus = 0f;
        UpdateEffectiveDamage();
    }
    private void UpdateEffectiveDamage()
    {
        damage = baseDamage + damageBuffBonus;

        if (damage < 0f)
        {
            damage = 0f;
        }
    }

}

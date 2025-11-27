using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public float currentHealth;

    [SerializeField] private float maxHealth = 100f;


    private void Awake()
    {
        currentHealth = maxHealth;

    }


    public void TakeDamage(float damage) {

        if(damage <=0.0f) return;


        currentHealth -= damage;
        Debug.Log("Health:" + currentHealth); 

        if (currentHealth <= 0.0f) {

            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);

    }




}

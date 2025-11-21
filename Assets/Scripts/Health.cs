using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    private float maxHealth = 100.0f;
    private float currentHealth = 100.0f;
    private TextMeshProUGUI healthText;

  

    public void increaseHealth()
    {
        maxHealth += 20;
        currentHealth = maxHealth;
        healthText.text = "Health " + currentHealth;
    
    
    }

   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

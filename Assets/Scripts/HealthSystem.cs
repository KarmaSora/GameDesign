using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public float health;


    public void TakeDamage(float damage) {

        health -=damage;
        Debug.Log("Health:" + health); 

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;


    private void OnTriggerEnter(Collider other)
    {
        bool kill = false;
        if (other.CompareTag("Enemy"))
        {
            HealthSystem enemy = other.GetComponent<HealthSystem>();
            enemy.TakeDamage(damage);
            if(enemy.health <= 0)
            {
                kill = true;
            }
        }
        if(kill) Destroy(other.gameObject);
    }

}

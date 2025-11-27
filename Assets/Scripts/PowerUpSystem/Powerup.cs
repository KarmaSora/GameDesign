using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter(Collider other)
    {
        //add check if its a player or enemy

        Destroy(gameObject);
        powerupEffect.Apply(other.gameObject);
    }

}

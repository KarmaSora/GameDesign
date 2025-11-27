using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Powerups/Healthbuff")]
public class HealthBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<HealthSystem>().currentHealth += amount;
        Debug.Log("HealhBuff Activated increase by " + amount);

    }
}

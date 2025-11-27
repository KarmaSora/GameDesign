using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/JumpBuff")]
public class JumpBuff : PowerupEffect
{
    [Header("Jump Buff Settings")]
    public float amount = 5f;
    public float duration = 3f;  // How long the buff lasts

    public override void Apply(GameObject target)
    {
        PlayerMovement pm = target.GetComponent<PlayerMovement>();

        if (pm == null)
        {
            Debug.LogWarning("JumpBuff: Target has no PlayerMovement component.");
            return;
        }

        pm.StartCoroutine(ApplyJumpBuff(pm));
    }

    private IEnumerator ApplyJumpBuff(PlayerMovement pm)
    {
        // 1. Apply
        pm.jumpIncreaser += amount;
        Debug.Log("JumpBuff activated: +" + amount + " jump for " + duration + " seconds.");

        // 2. Wait
        yield return new WaitForSeconds(duration);

        // 3. Revert
        pm.jumpIncreaser -= amount;
        Debug.Log("JumpBuff expired: -" + amount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void TakeDamage(Attack attacker)
    {
        if (PlayerController.instance.isBlock)
            currentHealth += attacker.damage/2;

        else
        {
            currentHealth -= attacker.damage;
            transform.GetComponent<Rigidbody2D>().AddForce((transform.position - attacker.transform.position).normalized * attacker.attackForce);
            Hurt?.Invoke();
            FarmingManager.instance.IntervalDamage(attacker);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public float hitpoint;
    public float maxhitpoint;

    private float lastImmune;
    private float immuneTime = 0.1f;

    protected virtual void ReceiveDamage(Damage dmg)
    {

        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 10, Color.red, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.up * 25, 0.5f) ;

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {

    }
}

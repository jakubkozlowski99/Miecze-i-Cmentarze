using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public float hitpoint;
    public float maxhitpoint;

    protected float lastImmune;
    protected float immuneTime = 0.1f;

    protected virtual void ReceiveDamage(Damage dmg)
    {

        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 10, Color.red, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.up * 25, 0.5f) ;
            AudioManager.instance.Play("hit");

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

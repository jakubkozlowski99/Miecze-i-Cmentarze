using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public float hitpoint;
    public float maxhitpoint;

    public float textOffset = 0.5f;

    protected float lastImmune;
    protected float immuneTime = 0.1f;

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;

            int rand = Random.Range(1, 100);
            int critChance = ((int)Mathf.Round(dmg.critChance));

            float bonusDamage = dmg.damageAmount * ((dmg.armorPenetration - dmg.damageReduction) / 100);

            float damageDealt = Mathf.Round(dmg.damageAmount + bonusDamage);
            if (damageDealt < 0)
            {
                damageDealt = 0;
            }

            if (rand <= critChance)
            {
                hitpoint -= (damageDealt * 2);
                GameManager.instance.ShowText((damageDealt *2).ToString(), 12, Color.red, new Vector3(transform.position.x, transform.position.y + textOffset, transform.position.z), Vector3.up * 25, 0.5f, false);
            }
            else
            {
                hitpoint -= damageDealt;
                GameManager.instance.ShowText(damageDealt.ToString(), 10, Color.red, new Vector3(transform.position.x, transform.position.y + textOffset, transform.position.z), Vector3.up * 25, 0.5f, false);
            }

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

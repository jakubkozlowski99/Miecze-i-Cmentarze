using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Collidable
{
    public int damagePoint = 1;
    public float lastDamage;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "PlayerHitbox")
            {
                Damage dmg = new Damage
                {
                    damageAmount = damagePoint
                };
                dmg.damageAmount -= GameManager.instance.player.playerStats.defense;
                if (dmg.damageAmount < 0) dmg.damageAmount = 0;
                GameManager.instance.player.SendMessage("ReceiveDamage", dmg);
            }
            else return;
        }
        else return;
    }
}

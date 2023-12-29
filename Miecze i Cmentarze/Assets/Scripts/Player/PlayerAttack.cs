using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Collidable
{
    public PlayerStats playerStats;
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
        if (coll.tag == "Fighter" && coll.name == "EnemyHitbox")
        {
            if (coll.name == "Player" || coll.name == "PlayerHitbox")
                return;

            Damage dmg = new Damage
            {
                damageAmount = GameManager.instance.player.playerStats.basicDamage,
                critChance = GameManager.instance.player.playerStats.critChance,
                armorPenetration = GameManager.instance.player.playerStats.armorPenetration
            };
            //Debug.Log(coll.name);
            coll.transform.parent.SendMessage("ReceiveDamage", dmg);
        }

    }
}

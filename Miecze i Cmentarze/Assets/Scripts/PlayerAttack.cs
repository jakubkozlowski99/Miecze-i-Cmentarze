using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Collidable
{
    public PlayerStats playerStats;
    private float damagePoint = 25;
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
                damageAmount = Mathf.Round((damagePoint * (1 + GameManager.instance.player.playerStats.attack / 10)))
            };
            //Debug.Log(coll.name);
            coll.transform.parent.SendMessage("ReceiveDamage", dmg);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : Collidable
{
    private Animator anim;
    private bool isHurt;
    private float attackCooldown = 1;
    private float lastAttack;

    protected override void Start()
    {
        base.Start();
        anim = transform.GetComponentInParent<Animator>();
    }

    protected override void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hurt") || anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_death") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_dead"))
        {
            isHurt = true;
        }
        else isHurt = false;
        base.Update();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (!isHurt && Time.time - lastAttack > attackCooldown)
        {
            if (coll.tag == "Fighter" && coll.name == "PlayerHitbox")
            {
                lastAttack = Time.time;
                anim.SetTrigger("Attack1");
            }
        }
        else return;
    }


}

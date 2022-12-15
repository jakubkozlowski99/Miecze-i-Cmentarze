using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : Collidable
{
    private Animator anim;
    private bool isHurt;
    private float attackCooldown = 1;
    private float lastAttack;
    private int attackAnimIndex;
    public int animationsAmount;

    protected override void Start()
    {
        base.Start();
        attackAnimIndex = 1;
        anim = transform.GetComponentInParent<Animator>();
    }

    protected override void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Death") || 
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
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
                SetAttackAnim();
            }
        }
        else return;
    }

    private void SetAttackAnim()
    {
        anim.SetTrigger("Attack" + attackAnimIndex);
        if (attackAnimIndex >= animationsAmount)
        {
            attackAnimIndex = 1;
        }
        else attackAnimIndex++;
    }
}

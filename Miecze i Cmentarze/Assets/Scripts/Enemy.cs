using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    public int xpValue = 15;
    public int coinsValue = 50;

    public float triggerLength = 1;
    public float chaseLength = 5;
    public float attackCooldown = 1.5f;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    public float enemyYSpeed = 1;
    public float enemyXSpeed = 1.5f;
    private string attack_1 = "enemy_atk1";
    private string hurt = "enemy_hurt";
    private string death = "enemy_death";
    private string attackTrigger_1 = "Attack1";
    private string hurtTrigger = "Hurt";
    private string deathTrigger = "Death";
    private bool enemyIsAttacking;
    private bool enemyIsHurt;
    private bool alive = true;
    private bool sortChanged;


    public ContactFilter2D filter;
    private Collider2D[] hits = new Collider2D[20];
    private Animator anim;
    public EnemyHealthBarBehaviour healthBar;
    private SpriteRenderer render;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        anim = GetComponent<Animator>();
        healthBar.SetHealth(hitpoint, maxhitpoint);
        render = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(attack_1))
            {
                enemyIsAttacking = true;
            }
            else enemyIsAttacking = false;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName(hurt))
            {
                enemyIsHurt = true;
            }
            else enemyIsHurt = false;

            if ((Vector3.Distance(playerTransform.position, startingPosition) < chaseLength) && !enemyIsAttacking && !enemyIsHurt)
            {
                if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                {
                    chasing = true;
                    if (!collidingWithPlayer) anim.SetBool("EnemyRun", true);
                }

                if (chasing)
                {
                    if (!collidingWithPlayer)
                    {
                        UpdateMotor((playerTransform.position - transform.position).normalized, enemyYSpeed, enemyXSpeed);
                    }
                }
                else
                {
                    //UpdateMotor(startingPosition - transform.position, enemyYSpeed, enemyXSpeed);
                }
            }
            else if (!enemyIsAttacking && !enemyIsHurt)
            {
                //UpdateMotor(startingPosition - transform.position, enemyYSpeed, enemyXSpeed);
                chasing = false;
                startingPosition = transform.position;
                anim.SetBool("EnemyRun", false);
            }

            collidingWithPlayer = false;
            boxCollider.OverlapCollider(filter, hits);
            sortChanged = false;
            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i] == null)
                {
                    continue;
                }

                if (hits[i].tag == "Fighter" && hits[i].name == "PlayerHitbox")
                {
                    collidingWithPlayer = true;
                }

                hits[i] = null;
            }
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_dead"))
            {
                GameManager.instance.coins += coinsValue;
                GameManager.instance.experience += xpValue;
                if (GameManager.instance.experience >= GameManager.instance.xpTable[GameManager.instance.playerLevel - 1])
                {
                    GameManager.instance.player.LevelUp();
                }
                GameManager.instance.ShowText("+" + xpValue + "xp", 10, Color.magenta, transform.position, Vector3.up * 40, 0.5f);
                GameManager.instance.player.xpBar.setXpBar();
                Destroy(gameObject);
            }
        }
    }
    private void Attack()
    {
        anim.SetTrigger(attackTrigger_1);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (alive)
        {
            base.ReceiveDamage(dmg);
            healthBar.SetHealth(hitpoint, maxhitpoint);
            anim.SetTrigger(hurtTrigger);
        }
    }

    protected override void Death()
    {
        alive = false;
        anim.SetTrigger(deathTrigger);
    }
}

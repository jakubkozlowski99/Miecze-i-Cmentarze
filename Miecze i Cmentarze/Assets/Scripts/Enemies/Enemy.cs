using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Mover
{
    #region Stats variables
    public int xpValue = 15;
    public int coinsValue = 50;
    public float damageReduction = 0;
    #endregion

    #region Movement variables
    //[SerializeField] protected Transform target;
    protected NavMeshAgent agent;
    public float angle = 0;

    public float triggerLength = 1;
    public float chaseLength = 5;
    public float attackCooldown = 1.5f;
    public float movementSpeed = 2f;
    protected bool chasing;
    protected bool collidingWithPlayer;
    protected Transform playerTransform;
    protected Vector3 startingPosition;
    protected string hurtTrigger = "Hurt";
    protected string deathTrigger = "Death";
    protected bool enemyIsAttacking;
    protected bool enemyIsHurt;
    protected bool alive = true;
    #endregion

    #region Collider variables
    [HideInInspector]
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[20];
    public Animator anim;
    public EnemyHealthBarBehaviour healthBar;
    #endregion

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
        healthBar = GetComponentInChildren<EnemyHealthBarBehaviour>();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        anim = GetComponent<Animator>();
        if (healthBar != null) healthBar.SetHealth(hitpoint, maxhitpoint);
    }

    protected virtual void FixedUpdate()
    {
        if (alive)
        {
            CheckAnimations();

            if ((Vector3.Distance(playerTransform.position, startingPosition) < chaseLength) && !enemyIsAttacking && !enemyIsHurt)
            {
                if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                {
                    SetChasing(true);
                }

                if (chasing)
                {
                    if (!collidingWithPlayer)
                    {
                        agent.updatePosition = true;
                        agent.SetDestination(playerTransform.position);

                        if (agent.path.corners.Length > 1)
                        {
                            agent.speed = CalculateSpeed();
                            TurnEnemy();
                        }
                    }
                }
            }
            else if ((!enemyIsAttacking && !enemyIsHurt) || !GameManager.instance.player.alive)
            {
                SetChasing(false);
            }

            collidingWithPlayer = false;
            boxCollider.OverlapCollider(filter, hits);
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
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
            {
                KillReward();
            }
        }
    }

    protected virtual void SetChasing(bool shouldChase)
    {
        if (shouldChase == false)
        {
            chasing = false;
            startingPosition = transform.position;
            agent.SetDestination(startingPosition);
            anim.SetBool("EnemyRun", false);
        }
        else
        {
            chasing = true;
            if (!collidingWithPlayer) anim.SetBool("EnemyRun", true);
        }
    }

    protected virtual void TurnEnemy()
    {
        if (agent.path.corners[1].x > transform.position.x)
        {
            if (transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }
        else if (agent.path.corners[1].x < transform.position.x) 
        {
            if (transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }
    }

    protected virtual void CheckAnimations()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            enemyIsAttacking = true;
            agent.updatePosition = false;
            agent.SetDestination(transform.position);
        }
        else enemyIsAttacking = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
        {
            enemyIsHurt = true;
            agent.updatePosition = false;
            agent.SetDestination(transform.position);
        }
        else enemyIsHurt = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Death") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            agent.updatePosition = false;
            agent.SetDestination(transform.position);
        }
    }

    protected override float CalculateSpeed()
    {
        //measure angle between enemy's running direction and X
        angle = Vector2.Angle(transform.position - agent.path.corners[1], new Vector2(1, 0));

        //keeping angle under 90 degrees
        if (angle > 90) angle = 180 - angle;

        //calculating speed
        float currentSpeed = movementSpeed - (angle * (0.00278f * movementSpeed));

        return currentSpeed;
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (alive)
        {
            dmg.damageReduction = damageReduction;
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

    protected virtual void KillReward()
    {
        CheckQuestGoals();
        GameManager.instance.coins += coinsValue;
        GameManager.instance.experience += xpValue;
        if (GameManager.instance.experience >= GameManager.instance.xpTable[GameManager.instance.playerLevel - 1])
        {
            GameManager.instance.player.LevelUp();
        }
        GameManager.instance.ShowText("+" + xpValue + "xp", 10, Color.magenta, transform.position, Vector3.up * 40, 0.5f);
        GameManager.instance.player.xpBar.SetXpBar();
        Destroy(gameObject);
    }

    protected virtual void CheckQuestGoals()
    {
        foreach (Quest quest in GameManager.instance.playerQuests)
        {
            if (quest.completed == false)
            {
                foreach (QuestGoal questGoal in quest.goals)
                {
                    if (questGoal.killGoal != null)
                    {
                        if (questGoal.killGoal.name + "(Clone)" == transform.name && questGoal.completed == false)
                        {
                            questGoal.currentAmount++;
                            if (questGoal.currentAmount >= questGoal.requiredAmount) questGoal.completed = true;
                            quest.CheckGoals();
                        }
                    }
                }
            }
            /*if (quest.completed == false && quest.currentGoal.killGoal != null) 
            {
                if(quest.currentGoal.killGoal.name + "(Clone)" == transform.name && quest.currentGoal.completed == false)
                {
                    quest.currentGoal.currentAmount++;
                    if (quest.currentGoal.currentAmount >= quest.currentGoal.requiredAmount) quest.currentGoal.completed = true;
                    quest.CheckGoals();
                }

            }*/
        }
    }
}

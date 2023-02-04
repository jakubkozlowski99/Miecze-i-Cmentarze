using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public string bossName;
    private bool healthBarShown;

    public BossHealthBar bossHealthBar;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        anim = GetComponent<Animator>();
        healthBarShown = false;
        if (!alive) CheckQuestGoals();
        LoadBoss();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (alive)
        {
            if (Time.time - lastImmune > immuneTime)
            {
                lastImmune = Time.time;
                hitpoint -= dmg.damageAmount;

                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 10, Color.red, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.up * 25, 0.5f);
                AudioManager.instance.Play("hit");

                if (hitpoint <= 0)
                {
                    hitpoint = 0;
                    Death();
                }
            }
            bossHealthBar.SetHealth(hitpoint, maxhitpoint);

        }
    }

    protected override void FixedUpdate()
    {
        if (alive)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                enemyIsAttacking = true;
            }
            else enemyIsAttacking = false;

            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
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
            else if ((!enemyIsAttacking && !enemyIsHurt) || !GameManager.instance.player.alive)
            {
                //UpdateMotor(startingPosition - transform.position, enemyYSpeed, enemyXSpeed);
                chasing = false;
                startingPosition = transform.position;
                anim.SetBool("EnemyRun", false);
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

        if (chasing && !healthBarShown)
        {
            healthBarShown = true;
            bossHealthBar.gameObject.SetActive(true);
            bossHealthBar.bossName.text = bossName;
        }
        else if (!chasing && healthBarShown)
        {
            healthBarShown = false;
            bossHealthBar.gameObject.SetActive(false);
        }
    }

    protected override void Death()
    {
        alive = false;
        anim.SetTrigger(deathTrigger);
    }
    protected override void KillReward()
    {
        GameManager.instance.coins += coinsValue;
        GameManager.instance.experience += xpValue;
        if (GameManager.instance.experience >= GameManager.instance.xpTable[GameManager.instance.playerLevel - 1])
        {
            GameManager.instance.player.LevelUp();
        }
        GameManager.instance.ShowText("+" + xpValue + "xp", 10, Color.magenta, transform.position, Vector3.up * 40, 0.5f);
        GameManager.instance.player.xpBar.SetXpBar();
        SaveManager.instance.tempBosses.Add(new BossData(this));
        GameManager.instance.CheckQuestBosses();
        CheckPortals();
        Destroy(gameObject);
    }

    /*protected override void CheckQuestGoals()
    {
        foreach (Quest quest in GameManager.instance.playerQuests)
        {
            if (quest.completed == false)
            {
                foreach (QuestGoal questGoal in quest.goals)
                {
                    if (questGoal.killGoal != null)
                    {
                        if (questGoal.killGoal.name == transform.name && questGoal.completed == false)
                        {
                            questGoal.currentAmount++;
                            if (questGoal.currentAmount >= questGoal.requiredAmount) questGoal.completed = true;
                            quest.CheckGoals();
                        }
                    }
                }
            }*/
            /*if (quest.completed == false && quest.currentGoal.killGoal != null) 
            {
                if(quest.currentGoal.killGoal.name + "(Clone)" == transform.name && quest.currentGoal.completed == false)
                {
                    quest.currentGoal.currentAmount++;
                    if (quest.currentGoal.currentAmount >= quest.currentGoal.requiredAmount) quest.currentGoal.completed = true;
                    quest.CheckGoals();
                }

            }*/
        
    

    private void LoadBoss()
    {
        foreach (BossData boss in SaveManager.instance.tempBosses)
        {
            if (boss.bossName == name) 
            {
                Destroy(gameObject);
            }
        }
    }

    private void CheckPortals()
    {
        Portal[] portals = FindObjectsOfType<Portal>();

        foreach(Portal portal in portals)
        {
            portal.CheckPortal();
        }
    }
}

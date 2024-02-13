using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Boss : Enemy
{
    public string bossName;
    public string unlockedMapName;
    private bool healthBarShown;

    public List<PatrolCheckpoint> patrolCheckpoints;

    public BossHealthBar bossHealthBar;

    protected override void Start()
    {
        base.Start();

        //setting player and boss positions to measure distance between them
        startingPosition = transform.position;

        //hiding boss health bar
        healthBarShown = false;

        //if (!alive) CheckQuestGoals();

        //checking if boss was already killed
        LoadBoss();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        //do if boss is alive
        if (alive)
        {
            //do if immune time is finished
            if (Time.time - lastImmune > immuneTime)
            {
                //update immune time
                lastImmune = Time.time;

                //randomizing critical hit
                int rand = UnityEngine.Random.Range(1, 100);
                int critChance = ((int)Mathf.Round(dmg.critChance));

                dmg.damageReduction = damageReduction;

                //setting bonus damage based on basic damage, armor penetration and damage reduction
                float bonusDamage = dmg.damageAmount * ((dmg.armorPenetration - dmg.damageReduction) / 100);

                float damageDealt = Mathf.Round(dmg.damageAmount + bonusDamage);
                if (damageDealt < 0)
                {
                    damageDealt = 0;
                }

                if (rand <= critChance)
                {
                    hitpoint -= (damageDealt * 2);
                    gameManager.ShowText((damageDealt * 2).ToString(), 12, Color.red, new Vector3(transform.position.x, transform.position.y + textOffset, transform.position.z), Vector3.up * 25, 0.5f, false);
                }
                else
                {
                    hitpoint -= damageDealt;
                    gameManager.ShowText(damageDealt.ToString(), 10, Color.red, new Vector3(transform.position.x, transform.position.y + textOffset, transform.position.z), Vector3.up * 25, 0.5f, false);
                }

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
        /*if (alive)
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
                else
                {
                    //UpdateMotor(startingPosition - transform.position, enemyYSpeed, enemyXSpeed);
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
        }*/

        base.FixedUpdate();

        SetHealthBar();
    }

    protected override void Patrol()
    {
        if (patrolCheckpoints.Count < 2) return;

        var nextCheckpoint = patrolCheckpoints[nextCheckpointIndex];

        if (new Vector2(transform.position.x, transform.position.y) == nextCheckpoint.coordinates)
        {
            if (patrolTimer < nextCheckpoint.waitingTime && !agent.isStopped)
            {
                agent.isStopped = true;
                anim.SetBool("EnemyRun", false);
            }
            else if (patrolTimer >= nextCheckpoint.waitingTime)
            {
                patrolTimer = 0f;

                agent.isStopped = false;
                agent.SetDestination(nextCheckpoint.coordinates);

                if (agent.path.corners.Length > 1)
                {
                    agent.speed = CalculateSpeed() / 2;
                    TurnEnemy();
                }

                anim.SetBool("EnemyRun", true);

                if (nextCheckpointIndex == patrolCheckpoints.Count - 1)
                {
                    nextCheckpointIndex -= 1;
                    patrolReverseDirection = true;
                }
                else if (nextCheckpointIndex == 0)
                {
                    nextCheckpointIndex += 1;
                    patrolReverseDirection = false;
                }
                else if (!patrolReverseDirection) nextCheckpointIndex += 1;
                else nextCheckpointIndex -= 1;
            }
            else patrolTimer += Time.deltaTime;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(nextCheckpoint.coordinates);
            anim.SetBool("EnemyRun", true);
            if (agent.path.corners.Length > 1)
            {
                agent.speed = CalculateSpeed() / 2;
                TurnEnemy();
            }
        }

        //base.Patrol();
    }

    private void SetHealthBar()
    {
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
        gameManager.coins += coinsValue;
        gameManager.experience += xpValue;
        if (gameManager.experience >= gameManager.xpTable[gameManager.playerLevel - 1])
        {
            gameManager.player.LevelUp();
        }
        if (unlockedMapName != "none") 
        {
            gameManager.mapsUnlocked.Add(unlockedMapName);
        }
        gameManager.ShowText("+" + xpValue + "xp", 10, Color.magenta, transform.position, Vector3.up * 40, 0.5f, false);
        gameManager.player.xpBar.SetXpBar();
        SaveBoss(true);
        gameManager.CheckQuestBosses();
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
        
    

    public void SaveBoss(bool isDead)
    {
        var boss = Array.Find(saveManager.tempBosses.ToArray(), boss => boss.bossName == name);

        if (boss != null) saveManager.tempBosses.Remove(boss);

        saveManager.tempBosses.Add(new BossData(this, isDead));
    }

    private void LoadBoss()
    {
        var shouldDelete = Array.Exists(saveManager.tempBosses.ToArray(), boss => boss.bossName == name && boss.dead);

        if (shouldDelete) Destroy(gameObject);
        else
        {
            var bossData = Array.Find(saveManager.tempBosses.ToArray(), boss => boss.bossName == name);

            if (bossData != null)
            {
                transform.position = new Vector3(bossData.posX, bossData.posY, 0);
                transform.localScale = new Vector3(bossData.scaleX, transform.localScale.y, transform.localScale.z);

                patrolReverseDirection = bossData.patrolReverseDirection;
                nextCheckpointIndex = bossData.nextCheckpointIndex;
                patrolTimer = bossData.patrolTimer;
                afterChasingTimer = bossData.afterChasingTimer;
            }
        }
        /*foreach (BossData boss in SaveManager.instance.tempBosses)
        {
            if (boss.bossName == name) 
            {
                Destroy(gameObject);
            }
        }*/
    }

    private void CheckPortals()
    {
        Portal[] portals = FindObjectsOfType<Portal>();

        foreach (Portal portal in portals) 
        {
            portal.CheckPortal();
        }
    }
}

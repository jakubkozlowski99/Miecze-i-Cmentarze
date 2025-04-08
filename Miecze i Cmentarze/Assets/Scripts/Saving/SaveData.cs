using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int sceneIndex;
    public float gameTimer;
    public List<string> mapsUnlocked;
    public PlayerData playerData;
    public InventoryData inventoryData;
    public List<ChestData> chestData;
    public List<QuestData> questData;
    public List<QuestData> completedQuestData;
    public List<ShrineData> shrineData;
    public List<SpawnerData> spawnerData;
    public List<BossData> bossData;
    public SaveData()
    {

    }
}

[Serializable]
public class PlayerData
{
    public int level;
    public int coins;
    public int xp;
    public int abilityPoints;
    public float hp;
    public float maxHP;
    public int stamina;
    public int maxStamina;
    public float playerX;
    public float playerY;

    //player attributes
    public float attack;
    public float speed;
    public float vitality;
    public float defense;

    public int addedAttackPoints;
    public int addedSpeedPoints;
    public int addedVitalityPoints;
    public int addedDefensePoints;

    public PlayerData(int level, int coins, int xp, int abilityPoints, float hp, float maxHP, int stamina, int maxStamina, Vector2 position,
        float attack, float speed, float vitality, float defense, int addedAttackPoints,
        int addedSpeedPoints, int addedVitalityPoints, int addedDefensePoints)
    {
        this.level = level;
        this.coins = coins;
        this.xp = xp;
        this.abilityPoints = abilityPoints;
        this.hp = hp;
        this.maxHP = maxHP;
        this.stamina = stamina;
        this.maxStamina = maxStamina;
        playerX = position.x;
        playerY = position.y;
        this.attack = attack;
        this.speed = speed;
        this.vitality = vitality;
        this.defense = defense;
        this.addedAttackPoints = addedAttackPoints;
        this.addedSpeedPoints = addedSpeedPoints;
        this.addedVitalityPoints = addedVitalityPoints;
        this.addedDefensePoints = addedDefensePoints;
        //CheckStats();
    }
    private void CheckStats()
    {
        foreach (var equippedItemSlot in InventoryUI.instance.equippedItemSlots)
        {
            if (equippedItemSlot.item != null)
            {
                CheckItemSlot(equippedItemSlot);
            }
        }
    }

    private void CheckItemSlot(EquippedItemSlot slot)
    {
        /*attack -= slot.item.attack;
        speed -= slot.item.speed;
        agility -= slot.item.agility;
        vitality -= slot.item.vitality;
        condition -= slot.item.condition;
        defense -= slot.item.defense;*/
    }

}

[Serializable]
public class InventoryData
{
    public List<string> itemNames;
    public List<string> equippedItemNames;

    public InventoryData(List<Item> items, List<Item> equippedItems)
    {
        itemNames = new List<string>();
        foreach (Item item in items)
        {
            itemNames.Add(item.name);
        }

        equippedItemNames = new List<string>();
        foreach (Item equippedItem in equippedItems) 
        {
            equippedItemNames.Add(equippedItem.name);
        }
    }
}

[Serializable]
public class ChestData
{
    public string chestName;
    public List<string> itemNames;

    public ChestData(string chestName, List<Item> items)
    {
        this.chestName = chestName;
        itemNames = new List<string>();

        foreach(Item item in items)
        {
            itemNames.Add(item.name);
        }
    }
}

[Serializable]
public class QuestData
{
    public string questName;
    public string questDescription;
    public string questDialogueText;
    public List<GoalData> goals;
    public bool completed;
    public bool rewardClaimed;
    public int xp;
    public int coins;

    public QuestData(Quest quest)
    {
        questName = quest.information.name;
        questDescription = quest.information.description;
        questDialogueText = quest.information.dialogueText;

        goals = new List<GoalData>();

        foreach (QuestGoal goal in quest.goals)
        {
            goals.Add(new GoalData(goal));
        }

        completed = quest.completed;
        rewardClaimed = quest.rewardClaimed;

        xp = quest.reward.xp;
        coins = quest.reward.coins;
    }
}

[Serializable]
public class GoalData
{
    public string killGoalName;
    public string itemGoalName;
    public string goalDescription;
    public int goalIndex;
    public int currentAmount;
    public int requiredAmount;
    public bool completed;

    public GoalData(QuestGoal goal)
    {
        if (goal.killGoal == null) killGoalName = "";
        else killGoalName = goal.killGoal.name;
        if (goal.itemGoal == null) itemGoalName = "";
        else itemGoalName = goal.itemGoal.name;
        goalDescription = goal.goalDescription;
        currentAmount = goal.currentAmount;
        requiredAmount = goal.requiredAmount;
        completed = goal.completed;
    }
}

[Serializable]
public class ShrineData
{
    public string shrineName;
    public bool collected;

    public ShrineData(Shrine shrine)
    {
        shrineName = shrine.name;
        collected = shrine.collected;
    }
}

[Serializable]
public class SpawnerData
{
    public string spawnerName;
    public float timer;
    public bool dead;
    public float lastTimerState;

    public float posX;
    public float posY;

    public float scaleX;

    public bool patrolReverseDirection;
    public int nextCheckpointIndex;
    public float patrolTimer;
    public float afterChasingTimer;

    public SpawnerData(EnemySpawner spawner)
    {
        spawnerName = spawner.name;
        timer = spawner.timer;
        dead = spawner.isDead;
        lastTimerState = GameManager.instance.gameTimer;
        if (spawner.transform.childCount > 0)
        {
            if (spawner.transform.GetChild(0) != null)
            {
                var enemy = spawner.transform.GetComponentInChildren<Enemy>();

                posX = enemy.transform.position.x;
                posY = enemy.transform.position.y;
                scaleX = enemy.transform.localScale.x;

                patrolReverseDirection = enemy.isPatrolReverseDirection;
                nextCheckpointIndex = enemy.nextCheckpointIndex;
                patrolTimer = enemy.patrolTimer;
                afterChasingTimer = enemy.afterChasingTimer;
            }
            else
            {
                posX = 0;
                posY = 0;
                scaleX = 0;

                patrolReverseDirection = false;
                nextCheckpointIndex = 0;
                patrolTimer = 0;
                afterChasingTimer = 15f;
            }
        }
    }
}

[Serializable]
public class BossData
{
    public string bossName;

    public bool isDead;

    public float posX;
    public float posY;

    public bool patrolReverseDirection;
    public int nextCheckpointIndex;
    public float patrolTimer;
    public float afterChasingTimer;
    public float scaleX;

    public BossData(Boss boss, bool isDead)
    {
        bossName = boss.name;
        this.isDead = isDead;

        if (!isDead)
        {
            posX = boss.transform.position.x;
            posY = boss.transform.position.y;
            scaleX = boss.transform.localScale.x;

            patrolReverseDirection = boss.isPatrolReverseDirection;
            nextCheckpointIndex = boss.nextCheckpointIndex;
            patrolTimer = boss.patrolTimer;
            afterChasingTimer = boss.afterChasingTimer;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int sceneIndex;
    public float gameTimer;
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
    public float agility;
    public float vitality;
    public float condition;
    public float defense;

    public int addedAttackPoints;
    public int addedSpeedPoints;
    public int addedAgilityPoints;
    public int addedVitalityPoints;
    public int addedConditionPoints;
    public int addedDefensePoints;

    public PlayerData(int level, int coins, int xp, int abilityPoints, float hp, float maxHP, int stamina, int maxStamina, Vector2 position,
        float attack, float speed, float agility, float vitality, float condition, float defense, int addedAttackPoints,
        int addedSpeedPoints, int addedAgilityPoints, int addedVitalityPoints, int addedConditionPoints, int addedDefensePoints)
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
        this.agility = agility;
        this.vitality = vitality;
        this.condition = condition;
        this.defense = defense;
        this.addedAttackPoints = addedAttackPoints;
        this.addedSpeedPoints = addedSpeedPoints;
        this.addedAgilityPoints = addedAgilityPoints;
        this.addedVitalityPoints = addedVitalityPoints;
        this.addedConditionPoints = addedConditionPoints;
        this.addedDefensePoints = addedDefensePoints;
        CheckStats();
    }
    private void CheckStats()
    {
        if (InventoryUI.instance.helmet.item != null)
        {
            CheckItemSlot(InventoryUI.instance.helmet);
        }
        if (InventoryUI.instance.weapon.item != null)
        {
            CheckItemSlot(InventoryUI.instance.weapon);
        }
        if (InventoryUI.instance.armor.item != null)
        {
            CheckItemSlot(InventoryUI.instance.armor);
        }
        if (InventoryUI.instance.ring.item != null)
        {
            CheckItemSlot(InventoryUI.instance.ring);
        }
        if (InventoryUI.instance.boots.item != null)
        {
            CheckItemSlot(InventoryUI.instance.boots);
        }
        if (InventoryUI.instance.gloves.item != null) 
        {
            CheckItemSlot(InventoryUI.instance.gloves);
        }
    }

    private void CheckItemSlot(EquippedItemSlot slot)
    {
        attack -= slot.item.attack;
        speed -= slot.item.speed;
        agility -= slot.item.agility;
        vitality -= slot.item.vitality;
        condition -= slot.item.condition;
        defense -= slot.item.defense;
    }

}

[Serializable]
public class InventoryData
{
    public List<string> itemNames;

    public string helmetName;
    public string weaponName;
    public string armorName;
    public string ringName;
    public string bootsName;
    public string glovesName;

    public InventoryData(List<Item> items, Item helmet, Item weapon, Item armor, Item ring, Item boots,
        Item gloves)
    {
        itemNames = new List<string>();
        foreach (Item item in items)
        {
            itemNames.Add(item.name);
        }

        if (helmet != null) helmetName = helmet.name;
        else helmetName = "";
        if (weapon != null) weaponName = weapon.name;
        else weaponName = "";
        if (armor != null) armorName = armor.name;
        else armorName = "";
        if (ring != null) ringName = ring.name;
        else ringName = "";
        if (boots != null) bootsName = boots.name;
        else bootsName = "";
        if (gloves != null) glovesName = gloves.name;
        else glovesName = "";
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

    public SpawnerData(EnemySpawner spawner)
    {
        spawnerName = spawner.name;
        timer = spawner.timer;
        dead = spawner.dead;
        lastTimerState = GameManager.instance.gameTimer;
    }
}

[Serializable]
public class BossData
{
    public string bossName;

    public BossData(Boss boss)
    {
        bossName = boss.name;
    }
}

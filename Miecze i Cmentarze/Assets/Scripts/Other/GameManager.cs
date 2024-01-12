using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        if (SaveManager.instance.isLoading) 
        { 
            LoadQuests();
            gameTimer = SaveManager.instance.tempTimer;
        }
        else gameTimer = 0;
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        
    }

    public List<int> xpTable = new List<int>();
    public Player player;

    public FloatingTextManager floatingTextManager;

    public int coins = 0;
    public int experience = 0;

    [SerializeField]
    public int playerLevel = 1;
    public int availablePoints = 0;

    [SerializeField]
    public List<Quest> playerQuests;

    [SerializeField]
    public List<Quest> completedQuests;

    [SerializeField]
    public List<string> mapsUnlocked;

    public float gameTimer;

    private void Update()
    {
        if (!PauseMenu.instance.gameIsPaused) gameTimer += Time.deltaTime;
    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration, bool onPlayer)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration, onPlayer);

        Debug.Log("text shown");
    }

    private void LoadQuests()
    {
        for (int i = 0; i < 2; i++) 
        {
            List<QuestData> data = new List<QuestData>();
            if (i == 0) data = SaveManager.instance.tempQuests;
            else data = SaveManager.instance.tempCompletedQuests;

            if(SaveManager.instance.tempCompletedQuests != null)
            {
                foreach (QuestData questData in data)
                {
                    Quest quest = new Quest();
                    quest.information.name = questData.questName;
                    quest.information.description = questData.questDescription;
                    quest.information.dialogueText = questData.questDialogueText;
                    quest.completed = questData.completed;
                    quest.rewardClaimed = questData.rewardClaimed;
                    quest.reward.xp = questData.xp;
                    quest.reward.coins = questData.coins;
                    quest.goals = new List<QuestGoal>();

                    foreach (GoalData goalData in questData.goals)
                    {
                        QuestGoal goal = new QuestGoal();
                        goal.goalDescription = goalData.goalDescription;
                        goal.completed = questData.completed;
                        if (goalData.itemGoalName != "")
                        {
                            goal.goalType = QuestGoal.GoalType.Gathering;
                        }
                        else goal.goalType = QuestGoal.GoalType.Kill;

                        foreach (Enemy enemy in SaveManager.instance.allEnemies)
                        {
                            if (goalData.killGoalName == enemy.name) goal.killGoal = enemy.gameObject;
                        }
                        foreach (Item item in SaveManager.instance.allItems)
                        {
                            if (goalData.itemGoalName == item.name) goal.itemGoal = item;
                        }
                        goal.currentAmount = goalData.currentAmount;
                        goal.requiredAmount = goalData.requiredAmount;
                        quest.goals.Add(goal);
                    }
                    if (i == 0) playerQuests.Add(quest);
                    else completedQuests.Add(quest);
                }
            }
            
        }
        /*if (SaveManager.instance.tempQuests != null)
        {
            foreach (QuestData questData in SaveManager.instance.tempQuests)
            {
                Quest quest = new Quest();
                quest.information.name = questData.questName;
                quest.information.description = questData.questDescription;
                quest.information.dialogueText = questData.questDialogueText;
                quest.completed = questData.completed;
                quest.rewardClaimed = questData.rewardClaimed;
                quest.reward.xp = questData.xp;
                quest.reward.coins = questData.coins;
                quest.goals = new List<QuestGoal>();

                foreach (GoalData goalData in questData.goals)
                {
                    QuestGoal goal = new QuestGoal();
                    goal.goalDescription = goalData.goalDescription;
                    goal.completed = goalData.completed;
                    if (goalData.itemGoalName != "")
                    {
                        goal.goalType = QuestGoal.GoalType.Gathering;
                    }
                    else goal.goalType = QuestGoal.GoalType.Kill;

                    foreach (Enemy enemy in SaveManager.instance.allEnemies)
                    {
                        if (goalData.killGoalName == enemy.name) goal.killGoal = enemy.gameObject;
                    }
                    foreach (Item item in SaveManager.instance.allItems)
                    {
                        if (goalData.itemGoalName == item.name) goal.itemGoal = item;
                    }
                    goal.currentAmount = goalData.currentAmount;
                    goal.requiredAmount = goalData.requiredAmount;

                    quest.goals.Add(goal);
                }
                playerQuests.Add(quest);
            }
        }*/
        
    }

    public void CheckQuestBosses()
    {
        foreach (Quest quest in playerQuests)
        {
            foreach (QuestGoal questGoal in quest.goals)
            {
                if (!questGoal.completed && questGoal.goalType == QuestGoal.GoalType.Kill)
                {
                    questGoal.currentAmount = 0;
                    foreach (BossData bossData in SaveManager.instance.tempBosses) 
                    {
                        if (bossData.bossName == questGoal.killGoal.name)
                        {
                            questGoal.currentAmount++;
                            if (questGoal.currentAmount >= questGoal.requiredAmount) questGoal.completed = true;
                        }
                    }
                    quest.CheckGoals();
                }
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }
}

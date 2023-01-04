using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        
        //if(SaveManager.instance.isLoading) SaveManager.instance.Load();
        foreach (Quest quest in playerQuests) quest.SetGoal();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
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

    public void Start()
    {
        
    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);

        Debug.Log("LoadState");
    }

    private void LoadQuests()
    {
        for(int i=0; i<2; i++)
        {
            List<QuestData> data = new List<QuestData>();
            if (i == 0) data = SaveManager.instance.tempQuests;
            else data = SaveManager.instance.tempCompletedQuests;

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
            playerQuests.Add(quest);
        }
    }
}

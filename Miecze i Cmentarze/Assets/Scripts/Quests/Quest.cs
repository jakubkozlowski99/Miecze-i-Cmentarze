using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [System.Serializable]
    public struct Info
    {
        public string name;
        public string description;
        public string dialogueText;
    }

    [Header("Info")] public Info information;

    [System.Serializable]
    public struct Stat
    {
        public int coins;
        public int xp;
    }

    public bool completed = false;

    public bool rewardClaimed = false;

    [Header("Reward")] public Stat reward = new Stat { coins = 10, xp = 10 };

    [SerializeField]
    public List<QuestGoal> goals;

    public QuestGoal currentGoal;

    public void SetGoal()
    {
        foreach(var goal in goals)
        {
            if (goal.completed == false)
            {
                currentGoal = goal;
                return;
            }
        }
    }

    public void CheckGoals()
    {
        completed = goals.TrueForAll(g => g.completed);
        SetGoal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public string name;
        public string description;
        public Sprite icon;
    }

    [Header("Info")] public Info information;

    [System.Serializable]
    public struct Stat
    {
        public int coins;
        public int xp;
    }

    public bool completed { get; private set; }
    public QuestCompletedEvent questCompleted;


    [Header("Reward")] public Stat reward = new Stat { coins = 10, xp = 10 };

    public abstract class QuestGoal : ScriptableObject
    {
        protected string description;
        public int currentAmount { get; protected set; }
        public int requiredAmount = 1;

        public bool completed { get; protected set; }
        [HideInInspector] public UnityEvent goalCompleted;

        public virtual string GetDescription()
        {
            return description;
        }

        public virtual void Initialize()
        {
            completed = false;
            goalCompleted = new UnityEvent();
        }

        protected void Evaluate()
        {
            if(currentAmount >= requiredAmount)
            {
                Complete();
            }
        }

        private void Complete()
        {
            completed = true;
            goalCompleted.Invoke();
            goalCompleted.RemoveAllListeners();
        }

        public void Skip()
        {
            Complete();
        }
    }

    public List<QuestGoal> goals;

    public void Initialize()
    {
        completed = false;
        questCompleted = new QuestCompletedEvent();

        foreach(var goal in goals)
        {
            goal.Initialize();
            goal.goalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }

    private void CheckGoals()
    {
        completed = goals.TrueForAll(g => g.completed);
        if (completed)
        {
            //give rewards
            questCompleted.Invoke(this);
            questCompleted.RemoveAllListeners();
        }
    }
}

public class QuestCompletedEvent : UnityEvent<Quest> { }

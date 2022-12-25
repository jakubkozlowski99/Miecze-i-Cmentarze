using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GameObject killGoal;
    public Item itemGoal;
    public int currentAmount;
    public int requiredAmount = 1;
    public string goalDescription;

    public bool completed;

    public enum GoalType
    {
        Kill,
        Gathering
    }

    public GoalType goalType;
}

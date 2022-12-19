using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    protected string description;
    public int currentAmount;
    public int requiredAmount = 1;

    public bool completed;

    public enum GoalType
    {
        Kill,
        Gathering
    }
}

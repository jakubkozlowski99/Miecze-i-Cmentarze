using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one inventory");
            return;
        }
        instance = this;
    }
    #endregion
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;
    public bool canToggle;

    public List<Item> items = new List<Item>();

    public void Add (Item item)
    {
        if (items.Count < space) items.Add(item);
        CheckQuestItems();
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }

    public void Remove (Item item)
    {
        items.Remove(item);
        CheckQuestItems();
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }

    public void CheckQuestItems()
    {
        foreach(Quest playerQuest in GameManager.instance.playerQuests)
        {
            foreach(QuestGoal questGoal in playerQuest.goals)
            {
                int questItemsAmount = 0;
                if (questGoal.itemGoal != null)
                {
                    foreach (Item item in items)
                    {
                        if (questGoal.itemGoal == item)
                        {
                            questItemsAmount++;
                        }
                    }
                    questGoal.currentAmount = questItemsAmount;
                    if (questGoal.currentAmount >= questGoal.requiredAmount) questGoal.completed = true;
                    else questGoal.completed = false;
                    playerQuest.CheckGoals();
                }
            }
        }
    }
}

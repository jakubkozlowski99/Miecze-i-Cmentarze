using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestsUI : MonoBehaviour
{
    public Transform questsArea;

    public GameObject questSlotPrefab;

    public List<QuestSlot> questSlots;

    public QuestSlot selectedQuest;

    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questGoals;

    public void AddQuest(Quest newQuest)
    {
        GameObject newQuestObject = Instantiate(questSlotPrefab, questsArea);
        newQuestObject.GetComponent<TextMeshProUGUI>().text = newQuest.information.name;

        QuestSlot newQuestSlot = newQuestObject.GetComponent<QuestSlot>();
        newQuestSlot.quest = newQuest;
        newQuestSlot.highlightImage.enabled = false;
        newQuestSlot.questsUI = this;
    }

    public void ShowDescription(Quest quest, QuestSlot questSlot)
    {
        questName.text = "";
        questDescription.text = "";
        questGoals.text = "";
        Debug.Log("dzie? dobry");
        if (selectedQuest != null) selectedQuest.highlightImage.enabled = false;
        selectedQuest = questSlot;
        questName.text = quest.information.name;
        questDescription.text = quest.information.description;

        foreach(QuestGoal questGoal in quest.goals)
        {
            questGoals.text += questGoal.goalDescription + ' ' + questGoal.currentAmount + '/' + questGoal.requiredAmount + '\n';
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public Quest quest;

    public Image highlightImage;

    public QuestsUI questsUI;

    public void ShowDescription()
    {
        highlightImage.enabled = true;
        Debug.Log("siema");
        questsUI.ShowDescription(quest, this);
    }
}

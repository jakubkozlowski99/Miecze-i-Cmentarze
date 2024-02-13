using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collidable
{
    protected bool playerNearby;
    protected bool textShown;
    public float interactionTextOffset;

    [Header("INK json")]
    public TextAsset inkJSON;

    public InteractionTextManager interactionTextManager;

    [Header("Shop")]
    protected Shop shop;

    public List<Item> shopItems;

    [SerializeField]
    public List<Quest> quests;

    public NPC thisNPC;

    public GameObject attentionMark;

    public DialogueManager dialogueManager;

    protected override void Start()
    {
        base.Start();

        dialogueManager = DialogueManager.instance;

        shop = new Shop();
        shop.shopItems = shopItems;
        LoadNPCQuests(gameManager.playerQuests, gameManager.completedQuests);
        SetAttentionMark();
    }
    protected override void Update()
    {
        playerNearby = IsPlayerNearby();

        if (playerNearby && !textShown && !dialogueManager.dialogueIsPlaying)
        {
            ShowInteractionText();
        }
        if ((textShown && !playerNearby) || dialogueManager.dialogueIsPlaying)
        {
            HideInteractionText();
        }

        if (inputHandler.CheckKey("Interaction") && playerNearby && !dialogueManager.dialogueIsPlaying)
        {
            OnActivation();
        }
    }
    protected void OnActivation()
    {
        Debug.Log(thisNPC.name);
        HideInteractionText();
        dialogueManager.EnterDialogueMode(inkJSON, transform.name, shop, FindQuest(), thisNPC);
    }

    protected void ShowInteractionText()
    {

        interactionTextManager.Show("[" + inputHandler.keyBinds.binds["Interaction"].ToString() + "] " + "Rozmawiaj",
            7, Color.yellow, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), interactionTextOffset);
        textShown = true;
    }

    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }

    public Quest FindQuest()
    {
        foreach(Quest quest in quests)
        {
            if (quest.rewardClaimed == false) return quest;
        }
        return null;
    }

    public void ClaimReward()
    {
        
    }

    public void LoadNPCQuests(List<Quest> playerQuests, List<Quest> completedQuests)
    {
        List<Quest> quests = new List<Quest>();
        int questIndex = 0;

        foreach (Quest quest in this.quests)
        {
            foreach(Quest completedQuest in completedQuests)
            {
                if (quest.information.name == completedQuest.information.name)
                {
                    quests.Add(completedQuest);
                    questIndex++;
                }
            }
        }

        foreach (Quest quest in this.quests)
        {
            foreach (Quest playerQuest in playerQuests)
            {
                if (quest.information.name == playerQuest.information.name)
                {
                    quests.Add(playerQuest);
                    questIndex++;
                }
            }
        }

        for (int i = questIndex; i < this.quests.Count; i++)
        {
            quests.Add(this.quests[i]);
        }

        this.quests = quests;
    }

    public void SetAttentionMark()
    {
        var quest = FindQuest();
        if (quest != null)
        {
            foreach (Quest playerQuest in gameManager.playerQuests)
            {
                if (playerQuest == quest)
                {
                    attentionMark.SetActive(false);
                    return;
                }
            }
            attentionMark.SetActive(true);
        }
        else attentionMark.SetActive(false);
    }
}

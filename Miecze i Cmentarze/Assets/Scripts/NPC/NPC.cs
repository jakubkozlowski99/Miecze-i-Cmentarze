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

    protected override void Start()
    {
        base.Start();
        shop = new Shop();
        shop.shopItems = shopItems;
    }
    protected override void Update()
    {
        playerNearby = IsPlayerNearby();

        if (playerNearby && !textShown && !DialogueManager.instance.dialogueIsPlaying)
        {
            ShowInteractionText();
        }
        if ((textShown && !playerNearby) || DialogueManager.instance.dialogueIsPlaying)
        {
            HideInteractionText();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNearby && !DialogueManager.instance.dialogueIsPlaying)
        {
            OnActivation();
        }
    }
    protected void OnActivation()
    {
        Debug.Log(thisNPC.name);
        HideInteractionText();
        DialogueManager.instance.EnterDialogueMode(inkJSON, transform.name, shop, FindQuest(), thisNPC);
    }

    protected void ShowInteractionText()
    {

        interactionTextManager.Show("[E] Rozmawiaj", 7, Color.yellow, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), interactionTextOffset);
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
}

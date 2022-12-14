using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collidable
{
    protected float animTime = 5;
    protected float lastAnim;

    protected bool playerNearby;
    protected bool textShown;
    public float interactionTextOffset;

    [Header("INK json")]
    public TextAsset inkJSON;

    public InteractionTextManager interactionTextManager;

    [Header("Shop")]
    protected Shop shop;

    public List<Item> shopItems;

    protected override void Start()
    {
        base.Start();
        shop = new Shop();
        shop.shopItems = shopItems;
    }
    protected override void Update()
    {
        playerNearby = IsPlayerNearby();

        if (playerNearby && !textShown)
        {
            ShowInteractionText();
        }
        if (textShown && !playerNearby)
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
        Debug.Log(inkJSON.text);
        HideInteractionText();
        DialogueManager.instance.EnterDialogueMode(inkJSON, transform.name, shop);
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
}

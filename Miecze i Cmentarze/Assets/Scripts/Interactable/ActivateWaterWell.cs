using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWaterWell : Collidable
{
    public InteractionTextManager interactionTextManager;
    public WaterWell waterWell;

    private bool playerNearby;
    private bool textShown;
    public float interactionTextOffset;

    private Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponentInParent<Animator>();
    }

    protected override void Update()
    {
        playerNearby = IsPlayerNearby();

        if (playerNearby && !textShown && waterWell.collectable)
        {
            ShowInteractionText();
        }
        if (textShown && !playerNearby)
        {
            HideInteractionText();
        }

        if (inputHandler.CheckKey(KeyActions.Interaction) && waterWell.collectable && playerNearby)
        {
            OnActivation();
        }
    }

    protected void OnActivation()
    {
        player.hitpoint = player.maxhitpoint;
        player.healthBar.SetValue(player.hitpoint);
        player.healthBar.SetText(player.hitpoint, player.maxhitpoint);
        anim.SetTrigger("Activation");
        interactionTextManager.Hide();
        audioManager.Play("heal");
    }

    protected void ShowInteractionText()
    {

        interactionTextManager.Show("[" + inputHandler.keyBinds.binds[KeyActions.Interaction].ToString() + "] " + "Wypij",
            7, Color.yellow, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), interactionTextOffset);
        textShown = true;
    }

    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }
}

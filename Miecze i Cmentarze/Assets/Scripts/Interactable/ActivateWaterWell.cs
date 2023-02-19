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

        if (Input.GetKeyDown(KeyCode.E) && waterWell.collectable && playerNearby)
        {
            OnActivation();
        }
    }

    protected void OnActivation()
    {
        GameManager.instance.player.hitpoint = GameManager.instance.player.maxhitpoint;
        GameManager.instance.player.healthBar.SetValue(GameManager.instance.player.hitpoint);
        GameManager.instance.player.healthBar.SetText(GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint);
        anim.SetTrigger("Activation");
        interactionTextManager.Hide();
        AudioManager.instance.Play("heal");
    }

    protected void ShowInteractionText()
    {

        interactionTextManager.Show("[E] Wypij", 7, Color.yellow, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), interactionTextOffset);
        textShown = true;
    }

    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }
}

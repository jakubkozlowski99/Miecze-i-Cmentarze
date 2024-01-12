using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShrine : Collidable
{
    public InteractionTextManager interactionTextManager;
    public Shrine shrine;

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

        if (playerNearby && !textShown && !shrine.collected)
        {
            ShowInteractionText();
        }
        if (textShown && !playerNearby)
        {
            HideInteractionText();
        }

        if (Input.GetKeyDown(KeyCode.E) && !shrine.collected && playerNearby)
        {
            OnActivation();
        }

    }

    protected void OnActivation()
    {
        anim.SetTrigger("Activation");
        shrine.collected = true;
        interactionTextManager.Hide();
        GameManager.instance.ShowText("+Punkt talentu", 7, Color.yellow, new Vector3(GameManager.instance.player.transform.position.x,
            GameManager.instance.player.transform.position.y + GameManager.instance.player.textOffset, GameManager.instance.player.transform.position.z), Vector3.up * 25, 0.5f, true);
        GameManager.instance.availablePoints++;
        shrine.LoadTempShrines(false);
        AudioManager.instance.Play("shrine");
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[E] Aktywuj", 7, Color.yellow, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }
}

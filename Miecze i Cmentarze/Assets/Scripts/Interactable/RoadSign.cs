using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSign : Collidable
{
    public InteractionTextManager interactionTextManager;
    public RoadSignUI roadSignUI;

    private bool playerNearby;
    private bool textShown;
    public float interactionTextOffset;

    protected override void Start()
    {
        base.Start();
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

        if (inputHandler.CheckKey(KeyActions.Interaction) && playerNearby)
        {
            OnActivation();
        }

    }
    protected void OnActivation()
    {
        roadSignUI = FindObjectOfType<RoadSignUI>();
        roadSignUI.ReadSign();
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[" + inputHandler.keyBinds.binds[KeyActions.Interaction].ToString() + "] " + "Zobacz",
            7, Color.yellow, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSign : Collidable
{
    public InteractionTextManager interactionTextManager;

    private bool playerNearby;
    private bool textShown;
    public float interactionTextOffset;

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

        if (Input.GetKeyDown(KeyCode.E) && playerNearby)
        {
            OnActivation();
        }

    }
    protected void OnActivation()
    {
        
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[E] Zobacz", 7, Color.yellow, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }
}

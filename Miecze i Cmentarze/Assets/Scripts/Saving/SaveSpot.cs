using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSpot : Collidable
{
    public InteractionTextManager interactionTextManager;

    private bool playerNearby;
    private bool textShown;
    public float interactionTextOffset;

    private bool saveDone;

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

        if (Input.GetKeyDown(KeyCode.E) && playerNearby)
        {
            OnActivation();
        }

    }
    protected void OnActivation()
    {
        if (!saveDone)
        {
            interactionTextManager.Hide();
            GameManager.instance.ShowText("Gra zapisana", 10, Color.yellow, new Vector3(GameManager.instance.player.transform.position.x,
                GameManager.instance.player.transform.position.y + 0.3f, GameManager.instance.player.transform.position.z), Vector3.up * 25, 0.5f);
            SaveManager.instance.Save();
            saveDone = true;
        }
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[E] Zapisz", 10, Color.yellow, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        saveDone = false;
        textShown = false;
        interactionTextManager.Hide();
    }
}
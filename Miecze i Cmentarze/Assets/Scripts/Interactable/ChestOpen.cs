using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : Collidable
{
    public InteractionTextManager interactionTextManager;
    public Chest chest;

    private bool playerNearby;
    public float interactionTextOffset;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        chest.chestOpened = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        playerNearby = IsPlayerNearby();
        
        if (playerNearby && !chest.textShown && chest.collectable)
        {
            ShowInteractionText();
        }
        if (chest.textShown && !playerNearby)
        {
            HideInteractionText();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNearby && chest.collectable)
        {
            GameManager.instance.player.canMove = false;
            Inventory.instance.canToggle = false;
            chest.anim.SetTrigger("Open");
            interactionTextManager.Hide();
            AudioManager.instance.Play("chest_open");
        }

        if (chest.anim.GetCurrentAnimatorStateInfo(0).IsName("chest_opened") && !chest.chestOpened)
        {
            chest.chestOpened = true;
            OnActivation();
        }
        if(chest.anim.GetCurrentAnimatorStateInfo(0).IsName("chest_idle") && chest.chestOpened)
        {
            chest.chestOpened = false;
        }

    }

    protected void OnActivation()
    {
        chest.chestUI.OpenChest(chest);
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[E] Otw�rz", 7, Color.yellow, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), interactionTextOffset);
        chest.textShown = true;
    }

    protected void HideInteractionText()
    {
        chest.textShown = false;
        interactionTextManager.Hide();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : Collidable
{
    protected float animTime = 5;
    protected float lastAnim;
    private Animator anim;

    private bool playerNearby;
    private bool textShown;
    public float interactionTextOffset;

    [Header("INK json")]
    public TextAsset inkJSON;

    public InteractionTextManager interactionTextManager;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
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

    protected void SetAnimation()
    {
        if (anim.GetBool("BlacksmithAnim") == true)
        {
            if (Time.time - lastAnim > animTime)
            {
                anim.SetBool("BlacksmithAnim", false);
                lastAnim = Time.time;
            }
        }
        else
        {
            if (Time.time - lastAnim > animTime)
            {
                anim.SetBool("BlacksmithAnim", true);
                lastAnim = Time.time;
            }
        }
    }

    protected void OnActivation()
    {
        Debug.Log(inkJSON.text);
        HideInteractionText();
        DialogueManager.instance.EnterDialogueMode(inkJSON, "Blacksmith");
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

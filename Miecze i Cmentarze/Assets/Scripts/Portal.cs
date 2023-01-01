using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public InteractionTextManager interactionTextManager;

    public int sceneIndex;

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
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[E] Wejdz", 7, Color.yellow, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }
}
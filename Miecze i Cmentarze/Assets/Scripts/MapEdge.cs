using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdge : Collidable
{
    private bool playerNearby;
    private bool textShown;
    public GameObject edgeText;

    protected override void Start()
    {
        base.Start();
        textShown = false;
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

    }

    protected void ShowInteractionText()
    {
        edgeText.SetActive(true);
        textShown = true;
        Debug.Log("siemexon");
    }
    protected void HideInteractionText()
    {
        textShown = false;
        edgeText.SetActive(false);
    }
}

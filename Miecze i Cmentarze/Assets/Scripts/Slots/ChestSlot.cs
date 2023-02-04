using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestSlot : Slot
{
    public ChestUI chestUI;

    public void ShowDetails()
    {
        if (!highlighted)
        {
            lastClick = Time.time;
            chestUI.RemoveHighlights();
            chestUI.chestInventoryItemDetailsUI.HideDetails();
            chestUI.chestItemDetailsUI.ShowDetails(item);
            highlightImage.enabled = true;
            highlighted = true;
        }
        else if (Time.time - lastClick < 0.5f && item != null)
        {
            highlighted = false;
            chestUI.chestItemDetailsUI.TakeItem();
        }
        else lastClick = Time.time;
    }
}

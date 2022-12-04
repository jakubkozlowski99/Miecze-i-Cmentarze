using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestSlot : Slot
{
    public ChestUI chestUI;

    public void ShowDetails()
    {
        chestUI.RemoveHighlights();
        chestUI.chestInventoryItemDetailsUI.HideDetails();
        chestUI.chestItemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }
}

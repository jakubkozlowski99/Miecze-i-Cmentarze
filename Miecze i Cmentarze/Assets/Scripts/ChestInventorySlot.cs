using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInventorySlot : Slot
{
    public ChestUI chestUI;
    public void ShowDetails()
    {
        chestUI.RemoveHighlights();
        chestUI.chestItemDetailsUI.HideDetails();
        chestUI.chestInventoryItemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }
}

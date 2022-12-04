using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Slot
{
    public InventoryUI inventoryUI;

    public int slotNumber;

    public void ShowDetails()
    {
        inventoryUI.RemoveHighlights();
        inventoryUI.equippedItemDetailsUI.HideDetails();
        inventoryUI.itemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }
}

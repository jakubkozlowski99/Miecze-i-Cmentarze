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
        if (!highlighted)
        {
            lastClick = Time.time;
            inventoryUI.RemoveHighlights();
            inventoryUI.equippedItemDetailsUI.HideDetails();
            inventoryUI.itemDetailsUI.ShowDetails(item);
            highlightImage.enabled = true;
            highlighted = true;
        }
        else if (Time.time - lastClick < 0.5f && item != null)
        {
            highlighted = false;
            inventoryUI.itemDetailsUI.UseItem(item.type);
        }
        else lastClick = Time.time;
    }
}

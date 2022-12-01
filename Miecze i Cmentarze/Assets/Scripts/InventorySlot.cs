using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public Image highlightImage;

    public InventoryUI inventoryUI;

    Item item;
    public int slotNumber;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void ShowDetails()
    {
        inventoryUI.RemoveHighlights();
        inventoryUI.equippedItemDetailsUI.HideDetails();
        inventoryUI.itemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }
}

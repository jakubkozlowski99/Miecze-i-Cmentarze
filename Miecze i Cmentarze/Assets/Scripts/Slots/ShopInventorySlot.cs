using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventorySlot : Slot
{
    public ShopUI shopUI;
    public void ShowDetails()
    {
        shopUI.sell = true;
        shopUI.RemoveHighlights();
        shopUI.shopItemDetailsUI.HideDetails();
        shopUI.shopInventoryItemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : Slot
{
    public ShopUI shopUI;
    public void ShowDetails()
    {
        shopUI.sell = false;
        shopUI.RemoveHighlights();
        shopUI.shopInventoryItemDetailsUI.HideDetails();
        shopUI.shopItemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }
}

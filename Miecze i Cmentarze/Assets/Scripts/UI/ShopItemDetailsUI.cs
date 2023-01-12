using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemDetailsUI : ItemDetailsUI
{
    public GameObject shopItemDetailsUI;
    public ShopUI shopUI;

    public bool sell;

    public override void ShowDetails(Item newItem)
    {
        item = newItem;

        if (item != null)
        {
            itemIcon.sprite = item.icon;
            itemName.text = item.name;
            GenerateDescription();
            shopItemDetailsUI.SetActive(true);
        }
        else
        {
            HideDetails();
        }
    }

    public override void HideDetails()
    {
        shopItemDetailsUI.SetActive(false);
    }

    public override void GenerateDescription()
    {
        base.GenerateDescription();

        if (shopUI.sell == true) itemDescription.text += "Cena: " + item.sellPrice;
        else itemDescription.text += "Cena: " + item.buyPrice;
    }

    public void BuyItem()
    {
        if (item.buyPrice <= GameManager.instance.coins && Inventory.instance.items.Count < Inventory.instance.space)
        {
            GameManager.instance.coins -= item.buyPrice;
            shopUI.inventory.Add(item);
            HideDetails();
            shopUI.RemoveHighlights();
            shopUI.UpdateShop();
            AudioManager.instance.Play("coins");
        }
    }

    public void SellItem()
    {
        shopUI.inventory.Remove(item);
        GameManager.instance.coins += item.sellPrice;
        HideDetails();
        shopUI.RemoveHighlights();
        shopUI.UpdateShop();
        AudioManager.instance.Play("coins");
    }
}

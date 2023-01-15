using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestItemDetailsUI : ItemDetailsUI
{
    public GameObject chestItemDetailsUI;
    public ChestUI chestUI;

    public override void ShowDetails(Item newItem)
    {
        item = newItem;

        if (item != null)
        {
            itemIcon.sprite = item.icon;
            itemName.text = item.name;
            GenerateDescription();
            chestItemDetailsUI.SetActive(true);
        }
        else
        {
            HideDetails();
        }
    }

    public override void HideDetails()
    {
        chestItemDetailsUI.SetActive(false);
    }

    public void TakeItem()
    {
        bool itemTaken = false;

        if (item.type == "coins")
        {
            int coinAmount;
            if (item.coins == 0) coinAmount = Random.Range(50, 151);
            else if (item.coins == 1) coinAmount = Random.Range(150, 501);
            else coinAmount = Random.Range(500, 2001);
            GameManager.instance.coins += coinAmount;
            itemTaken = true;
            AudioManager.instance.Play("coins");
        }
        else if (Inventory.instance.items.Count < Inventory.instance.space)
        {
            chestUI.inventory.Add(item);
            itemTaken = true;
            AudioManager.instance.Play("unequip");
        }
        else if (Inventory.instance.items.Count >= Inventory.instance.space)
        {
            AudioManager.instance.Play("denied");
        }

        if (itemTaken)
        {
            chestUI.chest.chestItems.Remove(item);
            HideDetails();
            chestUI.RemoveHighlights();
            chestUI.UpdateChest();

        }
    }

    public void PutItem()
    {
        chestUI.inventory.Remove(item);
        chestUI.chest.chestItems.Add(item);
        HideDetails();
        chestUI.RemoveHighlights();
        chestUI.UpdateChest();
    }
}

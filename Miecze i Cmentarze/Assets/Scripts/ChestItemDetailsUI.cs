using System.Collections;
using System.Collections.Generic;
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

    public override void GenerateDescription()
    {
        Debug.Log('\u015B');
        itemDescription.text = "";

        if (item.type == "weapon" || item.type == "armor" || item.type == "gloves" ||
            item.type == "boots" || item.type == "ring" || item.type == "helmet")
        {
            if (item.attack != 0) itemDescription.text += "Atak: +" + item.attack + "\n";
            if (item.speed != 0) itemDescription.text += "Szybko\u015B\u0107: +" + item.speed + "\n";
            if (item.agility != 0) itemDescription.text += "Zwinno\u015B\u0107: +" + item.agility + "\n";
            if (item.vitality != 0) itemDescription.text += "Witalno\u015B\u0107: +" + item.vitality + "\n";
            if (item.condition != 0) itemDescription.text += "Kondycja: +" + item.condition + "\n";
            if (item.defense != 0) itemDescription.text += "Obrona: +" + item.defense + "\n";
        }
        else if (item.type == "consumable")
        {

        }
        else if (item.type == "coins")
        {

        }
    }

    public void TakeItem()
    {
        chestUI.inventory.Add(item);
        chestUI.chest.chestItems.Remove(item);
        HideDetails();
        chestUI.RemoveHighlights();
        chestUI.UpdateChest();
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

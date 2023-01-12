using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    public Sprite unequippedImage;
    public Sprite equippedImage;

    public InventoryUI inventoryUI;
    
    public Image highlightImage;
    public Image itemImage;
    public Image slotImage;

    public Item item;

    public void OnEquip(Item newItem)
    {
        item = newItem;

        itemImage.sprite = item.icon;
        itemImage.enabled = true;

        GameManager.instance.player.playerStats.attack += item.attack;

        GameManager.instance.player.playerStats.speed += item.speed;

        GameManager.instance.player.playerStats.agility += item.agility;

        GameManager.instance.player.playerStats.vitality += item.vitality;

        GameManager.instance.player.playerStats.condition += item.condition;

        GameManager.instance.player.playerStats.defense += item.defense;

        GameManager.instance.player.playerStats.UpdateStats();

        slotImage.sprite = equippedImage;

        AudioManager.instance.Play("equip");
    }

    public void OnUnEquip()
    {
        GameManager.instance.player.playerStats.attack -= item.attack;

        GameManager.instance.player.playerStats.speed -= item.speed;

        GameManager.instance.player.playerStats.agility -= item.agility;

        GameManager.instance.player.playerStats.vitality -= item.vitality;

        GameManager.instance.player.playerStats.condition -= item.condition;

        GameManager.instance.player.playerStats.defense -= item.defense;

        GameManager.instance.player.playerStats.UpdateStats();

        Inventory.instance.Add(item);

        item = null;
        itemImage.enabled = false;
        slotImage.sprite = unequippedImage;

        AudioManager.instance.Play("unequip");
    }

    public void ShowDetails()
    {
        inventoryUI.RemoveHighlights();
        inventoryUI.itemDetailsUI.HideDetails();
        inventoryUI.equippedItemDetailsUI.ShowDetails(item);
        highlightImage.enabled = true;
    }

}

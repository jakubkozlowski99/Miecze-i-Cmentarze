using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsUI : MonoBehaviour
{
    public GameObject itemDetailsUI;
    public InventoryUI inventoryUI;

    public Text itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemIcon;

    public Item item;

    public virtual void ShowDetails(Item newItem)
    {
        item = newItem;

        if (item != null)
        {
            itemIcon.sprite = item.icon;
            itemName.text = item.name;
            GenerateDescription();
            itemDetailsUI.SetActive(true);
        }
        else
        {
            HideDetails();
        }
    }

    public virtual void HideDetails()
    {
        itemDetailsUI.SetActive(false);
    }

    public void UseItemButton()
    {
        UseItem(item.type);
    }

    public virtual void UseItem(string type)
    {
        if (item.type != "consumable")
        {
            foreach (var equippedItemSlot in inventoryUI.equippedItemSlots)
            {
                if (equippedItemSlot.itemType == type)
                {
                    if (equippedItemSlot.item != null) equippedItemSlot.OnUnEquip();
                    equippedItemSlot.OnEquip(item);
                }
            }
        }
        else
        {
            GameManager.instance.player.hitpoint += item.potionHp;
            if (GameManager.instance.player.hitpoint > GameManager.instance.player.maxhitpoint)
            {
                GameManager.instance.player.hitpoint = GameManager.instance.player.maxhitpoint;
            }
            GameManager.instance.player.healthBar.SetValue(GameManager.instance.player.hitpoint);
            GameManager.instance.player.healthBar.SetText(GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint);
            AudioManager.instance.Play("heal");
        }
        Inventory.instance.Remove(item);
        inventoryUI.UpdateInventory();
        inventoryUI.RemoveHighlights();
        HideDetails();
    }

    public void TakeOff()
    {
        if (Inventory.instance.items.Count < Inventory.instance.space)
        {
            if (item.type == "helmet")
            {
                inventoryUI.equippedItemSlots[0].OnUnEquip();
                inventoryUI.UpdateInventory();
                inventoryUI.RemoveHighlights();
                HideDetails();
            }
            if (item.type == "armor")
            {
                inventoryUI.equippedItemSlots[1].OnUnEquip();
                inventoryUI.UpdateInventory();
                inventoryUI.RemoveHighlights();
                HideDetails();
            }
            if (item.type == "gloves")
            {
                inventoryUI.equippedItemSlots[2].OnUnEquip();
                inventoryUI.UpdateInventory();
                inventoryUI.RemoveHighlights();
                HideDetails();
            }
            if (item.type == "weapon")
            {
                inventoryUI.equippedItemSlots[3].OnUnEquip();
                inventoryUI.UpdateInventory();
                inventoryUI.RemoveHighlights();
                HideDetails();
            }
            if (item.type == "ring")
            {
                inventoryUI.equippedItemSlots[4].OnUnEquip();
                inventoryUI.UpdateInventory();
                inventoryUI.RemoveHighlights();
                HideDetails();
            }
            if (item.type == "boots")
            {
                inventoryUI.equippedItemSlots[5].OnUnEquip();
                inventoryUI.UpdateInventory();
                inventoryUI.RemoveHighlights();
                HideDetails();
            }
        }
        else AudioManager.instance.Play("denied");
    }

    public virtual void RemoveItem()
    {
        Inventory.instance.Remove(item);
        inventoryUI.UpdateInventory();
        inventoryUI.RemoveHighlights();
        HideDetails();
    }

    public virtual void GenerateDescription()
    {
        itemDescription.text = "";

        if (item.type == "weapon" || item.type == "armor" || item.type == "gloves" || 
            item.type == "boots" || item.type == "ring" || item.type == "helmet")
        {
            if (item.basicDamage != 0) itemDescription.text += "Zadawane obrażenia: +" + item.basicDamage + "\n";
            if (item.armorPenetration != 0) itemDescription.text += "Penetracja pancerza: +" + item.armorPenetration + "%\n";
            if (item.bonusAttackSpeed != 0) itemDescription.text += "Szybkość ataku: +" + item.bonusAttackSpeed + "%\n";
            if (item.critChance != 0) itemDescription.text += "Szansa na cios krytyczny: +" + item.critChance + "%\n";
            if (item.bonusHp != 0) itemDescription.text += "Punkty zdrowia: +" + item.bonusHp + "\n";
            if (item.bonusHpRegen != 0) itemDescription.text += "Regeneracja zdrowia: +" + item.bonusHpRegen + "/5s\n";
            if (item.bonusSpeed != 0) itemDescription.text += "Szybkość ruchu: +" + item.bonusSpeed + "%\n";
            if (item.bonusStamina != 0) itemDescription.text += "Punkty kondycji: +" + item.bonusStamina + "\n";
            if (item.bonusStaminaRegen != 0) itemDescription.text += "Regeneracja kondycji: +" + item.bonusStaminaRegen + "%\n";
            if (item.damageReduction != 0) itemDescription.text += "Redukcja obrażeń: +" + item.damageReduction + "%\n"; 
        }
        else if(item.type == "consumable")
        {
            itemDescription.text += "Mikstura \u017Cycia" + "\n";
            itemDescription.text += "+" + item.potionHp + "HP" + "\n";
        }
        else if (item.type == "coins")
        {
            if (item.coins == 0) itemDescription.text += "Troch\u0119 z\u0142ota.";
            else if (item.coins == 1) itemDescription.text += "Sporo z\u0142ota.";
            else if (item.coins == 2) itemDescription.text += "Du\u017Co z\u0142ota.";
        }
    }
}
 
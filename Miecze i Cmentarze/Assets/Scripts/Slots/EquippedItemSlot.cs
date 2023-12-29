using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : Slot
{
    public string itemType;

    public Sprite unequippedImage;
    public Sprite equippedImage;

    public InventoryUI inventoryUI;
    public Image itemImage;
    public Image slotImage;

    public void OnEquip(Item newItem)
    {
        item = newItem;

        itemImage.sprite = item.icon;
        itemImage.enabled = true;

        //zrobic funkcje ktora przekazuje item i polepsza wszystkie staty na raz / albo nie

        GameManager.instance.player.playerStats.basicDamage += item.basicDamage;

        GameManager.instance.player.playerStats.armorPenetration += item.armorPenetration;

        GameManager.instance.player.playerStats.bonusAttackSpeed += item.bonusAttackSpeed;

        GameManager.instance.player.playerStats.critChance += item.critChance;

        GameManager.instance.player.playerStats.bonusHp += item.bonusHp;

        GameManager.instance.player.playerStats.bonusHpRegen += item.bonusHpRegen;

        GameManager.instance.player.playerStats.bonusSpeed += item.bonusSpeed;

        GameManager.instance.player.playerStats.bonusStamina += item.bonusStamina;

        GameManager.instance.player.playerStats.bonusStaminaRegen += item.bonusStaminaRegen;

        GameManager.instance.player.playerStats.damageReduction += item.damageReduction;

        GameManager.instance.player.playerStats.UpdateStats();

        slotImage.sprite = equippedImage;

        InventoryUI.instance.statsPanel.SetStatsText();

        AudioManager.instance.Play("equip");
    }

    public void OnUnEquip()
    {
        GameManager.instance.player.playerStats.basicDamage -= item.basicDamage;

        GameManager.instance.player.playerStats.armorPenetration -= item.armorPenetration;

        GameManager.instance.player.playerStats.bonusAttackSpeed -= item.bonusAttackSpeed;

        GameManager.instance.player.playerStats.critChance -= item.critChance;

        GameManager.instance.player.playerStats.bonusHp -= item.bonusHp;

        GameManager.instance.player.playerStats.bonusHpRegen -= item.bonusHpRegen;

        GameManager.instance.player.playerStats.bonusSpeed -= item.bonusSpeed;

        GameManager.instance.player.playerStats.bonusStamina -= item.bonusStamina;

        GameManager.instance.player.playerStats.bonusStaminaRegen -= item.bonusStaminaRegen;

        GameManager.instance.player.playerStats.damageReduction -= item.damageReduction;

        GameManager.instance.player.playerStats.UpdateStats();

        Inventory.instance.Add(item);

        item = null;
        itemImage.enabled = false;
        slotImage.sprite = unequippedImage;

        InventoryUI.instance.statsPanel.SetStatsText();

        AudioManager.instance.Play("unequip");
    }

    public void ShowDetails()
    {
        if (!highlighted)
        {
            lastClick = Time.time;
            inventoryUI.RemoveHighlights();
            inventoryUI.itemDetailsUI.HideDetails();
            inventoryUI.equippedItemDetailsUI.ShowDetails(item);
            highlightImage.enabled = true;
            highlighted = true;
        }
        else if (Time.time - lastClick < 0.5f && item != null)
        {
            highlighted = false;
            inventoryUI.equippedItemDetailsUI.TakeOff();
        }
        else lastClick = Time.time;
    }
}

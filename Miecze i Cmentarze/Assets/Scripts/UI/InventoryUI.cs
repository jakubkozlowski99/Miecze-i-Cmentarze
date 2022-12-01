using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public GameObject inventoryPanel;
    public InventorySlot[] slots;
    public ItemDetailsUI itemDetailsUI;
    public ItemDetailsUI equippedItemDetailsUI;

    public EquippedItemSlot helmet;
    public EquippedItemSlot weapon;
    public EquippedItemSlot armor;
    public EquippedItemSlot ring;
    public EquippedItemSlot boots;
    public EquippedItemSlot gloves;

    public Text abilityPoints;
    public Text level;
    public Text attack;
    public Text speed;
    public Text agility;
    public Text vitality;
    public Text condition;
    public Text defense;
    public Text coins;

    public GameObject attackButton;
    public GameObject speedButton;
    public GameObject agilityButton;
    public GameObject vitalityButton;
    public GameObject conditionButton;
    public GameObject defenseButton;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateInventory;
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNumber = i;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            UpdateInventory();
            RemoveHighlights();
            GameManager.instance.player.canMove = false;
            inventoryPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.player.canMove = true;
            inventoryPanel.SetActive(false);
            itemDetailsUI.HideDetails();
            equippedItemDetailsUI.HideDetails();
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        abilityPoints.text = GameManager.instance.availablePoints.ToString();
        level.text = GameManager.instance.playerLevel.ToString();
        attack.text = GameManager.instance.player.playerStats.attack.ToString();
        speed.text = GameManager.instance.player.playerStats.speed.ToString();
        agility.text = GameManager.instance.player.playerStats.agility.ToString();
        vitality.text = GameManager.instance.player.playerStats.vitality.ToString();
        condition.text = GameManager.instance.player.playerStats.condition.ToString();
        defense.text = GameManager.instance.player.playerStats.defense.ToString();
        coins.text = GameManager.instance.coins.ToString();

        UpdateAddPointButtons();
    }
    
    public void UpdateAddPointButtons()
    {
        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedAttackPoints < 9)
        {
            attackButton.SetActive(true);
        }
        else attackButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedSpeedPoints < 9)
        {
            speedButton.SetActive(true);
        }
        else speedButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedAgilityPoints < 9)
        {
            agilityButton.SetActive(true);
        }
        else agilityButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedVitalityPoints < 9)
        {
            vitalityButton.SetActive(true);
        }
        else vitalityButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedConditionPoints < 9)
        {
            conditionButton.SetActive(true);
        }
        else conditionButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedDefensePoints < 9)
        {
            defenseButton.SetActive(true);
        }
        else defenseButton.SetActive(false);
    }

    public void RemoveHighlights()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].highlightImage.enabled = false;
        }
        helmet.highlightImage.enabled = false;
        weapon.highlightImage.enabled = false;
        ring.highlightImage.enabled = false;
        armor.highlightImage.enabled = false;
        boots.highlightImage.enabled = false;
        gloves.highlightImage.enabled = false;
    }
}

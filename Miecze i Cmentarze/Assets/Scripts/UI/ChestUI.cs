using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public Inventory inventory;
    public Chest chest;
    public ChestItemDetailsUI chestItemDetailsUI;
    public ChestItemDetailsUI chestInventoryItemDetailsUI;

    public GameObject chestPanel;
    public ChestSlot[] chestSlots;
    public ChestInventorySlot[] chestInventorySlots;

    private void Start()
    {
        inventory = Inventory.instance;
    }

    public void OpenChest(Chest newChest)
    {
        chest = newChest;
        UpdateChest();
        RemoveHighlights();
        chestItemDetailsUI.HideDetails();
        chestInventoryItemDetailsUI.HideDetails();
        chestPanel.SetActive(true);
    }
    public void CloseChest()
    {
        Inventory.instance.canToggle = true;
        GameManager.instance.player.canMove = true;
        chestPanel.SetActive(false);
        chest.textShown = false;
        chest.anim.SetTrigger("Close");
    }

    public void UpdateChest()
    {
        for (int i = 0; i < chestInventorySlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                chestInventorySlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                chestInventorySlots[i].ClearSlot();
            }
        }
        for(int i = 0; i < chestSlots.Length; i++)
        {
            if (i < chest.chestItems.Count)
            {
                chestSlots[i].AddItem(chest.chestItems[i]);
            }
            else
            {
                chestSlots[i].ClearSlot();
            }
        }
    }

    public void RemoveHighlights()
    {
        for (int i = 0; i < chestInventorySlots.Length; i++)
        {
            chestInventorySlots[i].highlightImage.enabled = false;
        }
        for (int i = 0; i < chestSlots.Length; i++)
        {
            chestSlots[i].highlightImage.enabled = false;
        }
    }
}

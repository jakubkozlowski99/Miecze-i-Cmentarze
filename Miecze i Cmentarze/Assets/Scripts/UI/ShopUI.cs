using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Inventory inventory;
    public Shop shop;
    public Text coins;

    public ShopItemDetailsUI shopItemDetailsUI;
    public ShopItemDetailsUI shopInventoryItemDetailsUI;

    public ShopSlot[] shopSlots;
    public ShopInventorySlot[] shopInventorySlots;

    public GameObject shopPanel;

    public bool sell;

    public void Start()
    {
        inventory = Inventory.instance;
    }

    public void OpenShop(Shop newShop)
    {
        shop = newShop;
        UpdateShop();
        RemoveHighlights();
        shopItemDetailsUI.HideDetails();
        shopInventoryItemDetailsUI.HideDetails();
        shopPanel.SetActive(true);
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void UpdateShop()
    {
        for (int i = 0; i < shopInventorySlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                shopInventorySlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                shopInventorySlots[i].ClearSlot();
            }
        }
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i < shop.shopItems.Count)
            {
                shopSlots[i].AddItem(shop.shopItems[i]);
            }
            else
            {
                shopSlots[i].ClearSlot();
            }
        }

        coins.text = GameManager.instance.coins.ToString();
    }

    public void RemoveHighlights()
    {
        for (int i = 0; i < shopInventorySlots.Length; i++)
        {
            shopInventorySlots[i].highlightImage.enabled = false;
        }
        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].highlightImage.enabled = false;
        }
    }
}

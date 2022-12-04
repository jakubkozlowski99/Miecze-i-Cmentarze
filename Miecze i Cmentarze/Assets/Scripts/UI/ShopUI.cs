using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Inventory inventory;
    public Shop shop;

    public ShopSlot[] shopSlots;


    public GameObject shopPanel;
    public void Start()
    {
        inventory = Inventory.instance;
    }
}

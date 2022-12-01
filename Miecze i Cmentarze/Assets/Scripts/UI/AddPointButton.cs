using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointButton : MonoBehaviour
{
    public InventoryUI inventoryUI;
    private string stat;

    void Start()
    {
        stat = transform.parent.name;
    }

    public void AddPoint()
    {
        GameManager.instance.player.playerStats.AddPoint(stat);
        GameManager.instance.availablePoints--;
        inventoryUI.UpdateInventory();
    }
}

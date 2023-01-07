using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<Item> chestItems;
    public ChestUI chestUI;
    public Animator anim;
    public bool chestOpened;
    public bool collectable;
    public bool textShown;

    protected void Start()
    {
        anim = GetComponent<Animator>();
        chestUI = FindObjectOfType<ChestUI>();

        LoadTempChests(true);
    }

    protected void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("chest_idle")) collectable = true;
        else collectable = false;
    }

    public void LoadTempChests(bool loadData)
    {
        if (loadData) 
        {
            foreach (ChestData chest in SaveManager.instance.tempChests)
            {
                if (chest.chestName == name)
                {
                    chestItems = new List<Item>();
                    foreach (string itemName in chest.itemNames)
                    {
                        foreach (Item item in SaveManager.instance.allItems)
                        {
                            if (itemName == item.name) chestItems.Add(item);
                        }
                    }
                    SaveManager.instance.tempChests.Remove(chest);
                    break;
                }
            }
        }
        else
        {
            foreach (ChestData chest in SaveManager.instance.tempChests) 
            {
                if(chest.chestName == name)
                {
                    SaveManager.instance.tempChests.Remove(chest);
                    SaveManager.instance.tempChests.Add(new ChestData(name, chestItems));
                    return;
                }
            }
        }
        SaveManager.instance.tempChests.Add(new ChestData(name, chestItems));
    }
}

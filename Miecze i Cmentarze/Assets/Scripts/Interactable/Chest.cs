using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviourExtension
{
    public List<Item> chestItems;
    public ChestUI chestUI;
    public Animator anim;
    public bool chestOpened;
    public bool collectable;
    public bool textShown;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();

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
            foreach (ChestData chest in saveManager.tempChests)
            {
                if (chest.chestName == name)
                {
                    chestItems = new List<Item>();
                    foreach (string itemName in chest.itemNames)
                    {
                        foreach (Item item in saveManager.allItems)
                        {
                            if (itemName == item.name) chestItems.Add(item);
                        }
                    }
                    saveManager.tempChests.Remove(chest);
                    break;
                }
            }
        }
        else
        {
            foreach (ChestData chest in saveManager.tempChests) 
            {
                if(chest.chestName == name)
                {
                    saveManager.tempChests.Remove(chest);
                    saveManager.tempChests.Add(new ChestData(name, chestItems));
                    return;
                }
            }
        }
        saveManager.tempChests.Add(new ChestData(name, chestItems));
    }
}

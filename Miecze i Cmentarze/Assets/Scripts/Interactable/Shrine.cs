using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviourExtension
{
    public Animator anim;
    public bool collected;

    public Sprite collectedShrine;

    protected override void Start()
    {
        base.Start();
        LoadTempShrines(true);
        anim = GetComponent<Animator>();
        if (collected) anim.SetTrigger("ActivationOnStart");
    }

    public void LoadTempShrines(bool loadData)
    {
        if (loadData)
        {
            foreach (ShrineData shrine in saveManager.tempShrines)
            {
                if (name == shrine.shrineName)
                {
                    collected = shrine.collected;
                    saveManager.tempShrines.Remove(shrine);
                    break;
                }
            }
        }
        else
        {
            foreach (ShrineData shrine in saveManager.tempShrines)
            {
                if (name == shrine.shrineName)
                {
                    saveManager.tempShrines.Remove(shrine);
                    saveManager.tempShrines.Add(new ShrineData(this));
                    return;
                }
            }
        }
        saveManager.tempShrines.Add(new ShrineData(this));
    }
}

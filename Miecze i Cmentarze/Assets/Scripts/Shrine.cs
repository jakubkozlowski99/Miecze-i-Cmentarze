using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    public Animator anim;
    public bool collected;

    public Sprite collectedShrine;

    protected virtual void Start()
    {
        LoadTempShrines(true);
        anim = GetComponent<Animator>();
        if (collected) anim.SetTrigger("ActivationOnStart");
    }

    public void LoadTempShrines(bool loadData)
    {
        if (loadData)
        {
            foreach (ShrineData shrine in SaveManager.instance.tempShrines)
            {
                if (name == shrine.shrineName)
                {
                    collected = shrine.collected;
                    SaveManager.instance.tempShrines.Remove(shrine);
                    break;
                }
            }
        }
        else
        {
            foreach (ShrineData shrine in SaveManager.instance.tempShrines)
            {
                if (name == shrine.shrineName)
                {
                    SaveManager.instance.tempShrines.Remove(shrine);
                    SaveManager.instance.tempShrines.Add(new ShrineData(this));
                    return;
                }
            }
        }
        SaveManager.instance.tempShrines.Add(new ShrineData(this));
    }
}

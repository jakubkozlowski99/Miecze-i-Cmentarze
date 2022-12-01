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
    }

    protected void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("chest_idle")) collectable = true;
        else collectable = false;
    }
}

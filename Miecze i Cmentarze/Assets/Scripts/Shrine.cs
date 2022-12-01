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
        anim = GetComponent<Animator>();
        if (collected) anim.SetTrigger("ActivationOnStart");
    }
}

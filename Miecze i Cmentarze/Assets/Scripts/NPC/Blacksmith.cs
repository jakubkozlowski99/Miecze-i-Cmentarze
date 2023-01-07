using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : NPC
{
    public Animator anim;

    public float animTime = 5;
    private float lastAnim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        SetAnimation();
    }

    protected void SetAnimation()
    {
        if (anim.GetBool("BlacksmithAnim") == true)
        {
            if (Time.time - lastAnim > animTime)
            {
                anim.SetBool("BlacksmithAnim", false);
                lastAnim = Time.time;
            }
        }
        else
        {
            if (Time.time - lastAnim > animTime)
            {
                anim.SetBool("BlacksmithAnim", true);
                lastAnim = Time.time;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWell : MonoBehaviour
{
    public Animator anim;
    public bool collectable;

    public string noAnim = "water_well";

    protected void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(noAnim)) collectable = true;
        else collectable = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionText 
{
    public bool active;
    public GameObject go;
    public Text txt;
    public float offset;

    public void Show()
    {
        active = true;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateInteractionText()
    {
        //Debug.Log(go.transform.parent.transform.parent.transform.parent.name);
        if (!active)
            return;
        go.transform.position = Camera.main.WorldToScreenPoint(new Vector3(go.transform.parent.transform.parent.transform.parent.position.x
            , go.transform.parent.transform.parent.transform.parent.position.y + offset, go.transform.parent.transform.parent.transform.parent.position.z));

    }
}

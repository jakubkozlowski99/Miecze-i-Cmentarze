using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public bool onPlayer;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        if (onPlayer) onPlayer = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText(bool onPlayerIsBusy)
    {
        if (!active)
        {
            //if (onPlayerIsBusy) Show();
            return;
        }

        //Debug.Log(onPlayer);

        if (Time.time - lastShown > duration)
        {
            Hide();
        }

        go.transform.position += motion * Time.deltaTime;
    }
}

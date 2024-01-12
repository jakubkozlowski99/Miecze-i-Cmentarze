using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RoadSignSlot : MonoBehaviour
{
    public string slotName;
    public string slotTitle;
    public bool highlighted;
    public bool unlocked;

    public int sceneIndex;
    public float playerX;
    public float playerY;
    private float lastClick;

    public TextMeshProUGUI textBox;
    public Image slotImg;
    public Image highlight;
    public RoadSignUI UI;

    public void CheckIfUnlocked()
    {
        unlocked = Array.Exists(GameManager.instance.mapsUnlocked.ToArray(), mapName => mapName == slotName);

        if (unlocked)
        {
            textBox.text = slotTitle;
            slotImg.color = Color.white;
        }
        else
        {
            textBox.text = "ZABLOKOWANE";
            slotImg.color = Color.gray;
        }
    }

    public void OnClick()
    {
        if (!highlighted && unlocked)
        {
            lastClick = Time.time;
            UI.ClearHighlights();
            highlight.enabled = true;
            highlighted = true;
            UI.chosenSlot = this;
        }
        else if (Time.time - lastClick < 0.5f)
        {
            highlighted = false;
            UI.Travel();
        }
        else lastClick = Time.time;
    }
}

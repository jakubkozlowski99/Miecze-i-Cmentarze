using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

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
        foreach (var mapName in GameManager.instance.mapsUnlocked) 
        {
            if (slotName == mapName)
            {
                unlocked = true;
                textBox.text = slotTitle;
                slotImg.color = Color.white;
                return;
            }
        }
        unlocked = false;
        textBox.text = "ZABLOKOWANE";
        slotImg.color = Color.gray;
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadSignUI : MonoBehaviour
{
    public GameObject panel;
    public List<RoadSignSlot> slots;
    public RoadSignSlot chosenSlot;

    public void ReadSign()
    {
        GameManager.instance.player.canMove = false;
        Inventory.instance.canToggle = false;
        panel.SetActive(true);
        foreach(var slot in slots)
        {
            ClearHighlights();
            slot.CheckIfUnlocked();
        }
    }

    public void CloseSign()
    {
        GameManager.instance.player.canMove = true;
        Inventory.instance.canToggle = true;
        panel.SetActive(false);
    }

    public void ClearHighlights()
    {
        foreach(var slot in slots)
        {
            slot.highlight.enabled = false;
            slot.highlighted = false;
        }
    }
    public void Travel()
    {
        if (chosenSlot != null) 
        {
            panel.SetActive(false);
            AudioManager.instance.StopMusic("theme_" + SceneManager.GetActiveScene().buildIndex);
            SaveManager.instance.SaveSpawners();
            LevelLoader.instance.LoadLevel(chosenSlot.sceneIndex);
            GameManager.instance.player.transform.position = new Vector3(chosenSlot.playerX, chosenSlot.playerY, 0);
            GameManager.instance.player.canMove = true;
            Inventory.instance.canToggle = true;
        }
    }
}

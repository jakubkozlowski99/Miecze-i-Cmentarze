using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject deathScreen;

    public void BackToMenu()
    {
        Destroy(GameManager.instance.player.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(SaveManager.instance.gameObject);
        LevelLoader.instance.LoadLevel(0);
    }
}

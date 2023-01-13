using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Destroy(GameManager.instance.gameObject);
        SaveManager.instance.SaveGlobals();
        Destroy(SaveManager.instance.gameObject);
        AudioManager.instance.StopMusic("theme_" + SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        LevelLoader.instance.LoadLevel(0);
    }
}

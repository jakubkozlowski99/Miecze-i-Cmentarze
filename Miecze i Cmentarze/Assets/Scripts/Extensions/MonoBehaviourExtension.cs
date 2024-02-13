using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoBehaviourExtension : MonoBehaviour
{
    protected GameManager gameManager;
    protected AudioManager audioManager;
    protected SaveManager saveManager;
    protected PauseMenu pauseMenu;
    protected Player player;
    protected Inventory inventory;
    protected InputHandler inputHandler;

    protected virtual void Start()
    {
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
        saveManager = SaveManager.instance;
        pauseMenu = PauseMenu.instance;
        inventory = Inventory.instance;
        inputHandler = InputHandler.instance;

        if (SceneManager.GetActiveScene().buildIndex != 0 && gameObject.tag != "MainCamera") player = GameManager.instance.player;
    }
}

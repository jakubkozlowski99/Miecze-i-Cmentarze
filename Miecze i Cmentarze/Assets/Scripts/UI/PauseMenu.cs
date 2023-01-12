using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public Image background;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        background.enabled = false;
        Time.timeScale = 1f;
        gameIsPaused = false;
        AudioManager.instance.Play("unpause");
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        background.enabled = true;
        Time.timeScale = 0f;
        gameIsPaused = true;
        AudioManager.instance.Play("pause");
    }
}

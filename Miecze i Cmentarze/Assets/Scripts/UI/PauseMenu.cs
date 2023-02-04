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

    public GameObject settingsUI;

    public Slider musicVolume;
    public Slider soundsVolume;

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
        settingsUI.SetActive(false);
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

    public void Options()
    {
        settingsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        musicVolume.value = AudioManager.instance.musicVolume * 100;
        soundsVolume.value = AudioManager.instance.soundsVolume * 100;
    }

    public void Confirm()
    {
        AudioManager.instance.musicVolume = musicVolume.value / 100;
        AudioManager.instance.soundsVolume = soundsVolume.value / 100;
        AudioManager.instance.SetMusicVolume(AudioManager.instance.musicVolume);
        AudioManager.instance.SetVolume(AudioManager.instance.soundsVolume);

        SaveManager.instance.tempMusicVolume = AudioManager.instance.musicVolume;
        SaveManager.instance.tempSoundsVolume = AudioManager.instance.soundsVolume;
    }

    public void Back()
    {
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(false);
        background.enabled = false;
    }
}

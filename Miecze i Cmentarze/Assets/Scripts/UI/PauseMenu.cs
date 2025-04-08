using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public Image background;

    public GameObject settingsUI;
    public TextMeshProUGUI alertTxt;

    public Slider musicVolume;
    public Slider soundsVolume;

    private DateTime alertDuration;

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
            if (gameIsPaused) Resume();
            else Pause();
        }
        if (DateTime.Now > alertDuration) HideAlert();
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
        alertTxt.gameObject.SetActive(false);
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
        pauseMenuUI.SetActive(true);
        settingsUI.SetActive(false);
    }

    public void Save()
    {
        if (!GameManager.instance.playerInCombat)
        {
            pauseMenuUI.SetActive(false);
            settingsUI.SetActive(false);
            background.enabled = false;
            Time.timeScale = 1f;
            gameIsPaused = false;
            AudioManager.instance.Play("unpause");
            SaveManager.instance.Save();
            GameManager.instance.ShowText("Gra zapisana", 10, Color.yellow,
                new Vector3(GameManager.instance.player.transform.position.x,
                GameManager.instance.player.transform.position.y + GameManager.instance.player.textOffset,
                GameManager.instance.player.transform.position.z), Vector3.up * 25, 0.5f, true);
        }
        else ShowAlert("Nie zapisuj w trakcie walki", 3);
    }

    private void ShowAlert(string msg, double duration)
    {
        alertTxt.text = msg;
        alertDuration = DateTime.Now.AddSeconds(duration);
        alertTxt.gameObject.SetActive(true);
    }

    private void HideAlert() => alertTxt.gameObject.SetActive(false);
}

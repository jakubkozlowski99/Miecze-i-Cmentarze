using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject menu;
    public Slider musicVolumeSlider;
    public Slider soundsVolumeSlider;

    private void Start()
    {
        musicVolumeSlider.value = SaveManager.instance.tempMusicVolume * 100;
        soundsVolumeSlider.value = SaveManager.instance.tempSoundsVolume * 100;
    }

    public void Confirm()
    {
        AudioManager.instance.musicVolume = musicVolumeSlider.value / 100;
        AudioManager.instance.soundsVolume = soundsVolumeSlider.value / 100;
        AudioManager.instance.SetMusicVolume(AudioManager.instance.musicVolume);
        AudioManager.instance.SetVolume(AudioManager.instance.soundsVolume);

        SaveManager.instance.tempMusicVolume = AudioManager.instance.musicVolume;
        SaveManager.instance.tempSoundsVolume = AudioManager.instance.soundsVolume;
    }

    public void Back()
    {
        gameObject.SetActive(false);
        menu.SetActive(true);
    }
}

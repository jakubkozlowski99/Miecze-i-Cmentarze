using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject menu;
    public Slider musicVolumeSlider;
    public Slider soundsVolumeSlider;

    [SerializeField]
    public List<GameObject> sections;

    [SerializeField]
    public List<GameObject> settingsSectionButtons;

    private void Start()
    {
        musicVolumeSlider.value = SaveManager.instance.tempMusicVolume * 100;
        soundsVolumeSlider.value = SaveManager.instance.tempSoundsVolume * 100;
    }

    public void OpenSettings()
    {
        foreach (var section in sections) section.SetActive(false);
        foreach (var button in settingsSectionButtons) button.SetActive(true);
    }

    public void ConfirmVolumeSettings()
    {
        AudioManager.instance.musicVolume = musicVolumeSlider.value / 100;
        AudioManager.instance.soundsVolume = soundsVolumeSlider.value / 100;
        AudioManager.instance.SetMusicVolume(AudioManager.instance.musicVolume);
        AudioManager.instance.SetVolume(AudioManager.instance.soundsVolume);

        SaveManager.instance.tempMusicVolume = AudioManager.instance.musicVolume;
        SaveManager.instance.tempSoundsVolume = AudioManager.instance.soundsVolume;
    }

    public void ConfirmControlSettings()
    {
        // TO DO: method to confirm control settings on button
    }

    public void GoToSection(int sectionIndex)
    {
        sections[sectionIndex].SetActive(true);
        foreach (var button in settingsSectionButtons) button.SetActive(false);
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        menu.SetActive(true);
    }

    public void BackToSettingsSections()
    {
        sections.Find(section => section.activeInHierarchy).SetActive(false);
        //sections[currentSectionIndex].SetActive(false);
        foreach (var button in settingsSectionButtons) button.SetActive(true);
    }
}

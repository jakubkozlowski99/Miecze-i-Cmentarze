using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public GameObject continueButton;
    public GameObject settings;
    public GameObject help;

    private void Start()
    {
        if (UI.instance != null) Destroy(UI.instance.gameObject);

        if (!SaveManager.instance.FileExists(Application.persistentDataPath + "/" + "SaveTest.dat")) continueButton.SetActive(false);

        AudioManager.instance.PlayMusic("menu");

        if (FindObjectOfType<Player>() != null) Destroy(FindObjectOfType<Player>().gameObject);
    }
    public void Continue()
    {
        menu.SetActive(false);
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        SaveManager.instance.isLoading = true;
        AudioManager.instance.StopMusic("menu");
        AudioManager.instance.Play("confirm");
    }

    public void NewGame()
    {
        menu.SetActive(false);
        SaveManager.instance.ResetTemps();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        SaveManager.instance.isLoading = false;
        AudioManager.instance.StopMusic("menu");
        AudioManager.instance.Play("confirm");
    }

    public void Options()
    {
        settings.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Help()
    {
        help.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseHelp()
    {
        help.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviourExtension
{
    public GameObject menu;
    public GameObject continueButton;
    public GameObject settings;
    public GameObject help;

    protected override void Start()
    {
        base.Start();

        if (UI.instance != null) Destroy(UI.instance.gameObject);

        if (!saveManager.FileExists(Application.persistentDataPath + "/" + "SaveTest.dat")) continueButton.SetActive(false);

        audioManager.PlayMusic("menu");

        if (FindObjectOfType<Player>() != null) Destroy(FindObjectOfType<Player>().gameObject);
    }
    public void Continue()
    {
        menu.SetActive(false);
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(saveManager.tempSceneIndex);
        saveManager.isLoading = true;
        audioManager.StopMusic("menu");
        audioManager.Play("confirm");
    }

    public void NewGame()
    {
        CameraMotor.instance.lookAt = null;

        menu.SetActive(false);
        saveManager.ResetTemps();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        saveManager.isLoading = false;
        audioManager.StopMusic("menu");
        audioManager.Play("confirm");
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

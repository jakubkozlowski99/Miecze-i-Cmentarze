using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public GameObject continueButton;

    private void Start()
    {
        if (UI.instance != null) Destroy(UI.instance.gameObject);

        if (!SaveManager.instance.FileExists()) continueButton.SetActive(false);
    }
    public void Continue()
    {
        menu.SetActive(false);
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        SaveManager.instance.isLoading = true;
    }

    public void NewGame()
    {
        menu.SetActive(false);
        SaveManager.instance.ResetTemps();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        SaveManager.instance.isLoading = false;
    }
}

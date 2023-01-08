using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Continue()
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        SaveManager.instance.isLoading = true;
    }

    public void NewGame()
    {
        SaveManager.instance.ResetTemps();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        SaveManager.instance.isLoading = false;
    }
}

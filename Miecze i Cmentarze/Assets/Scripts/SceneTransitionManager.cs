using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.floatingTextManager = FindObjectOfType<FloatingTextManager>();
        AudioManager.instance.PlayMusic("theme_" + SceneManager.GetActiveScene().buildIndex);
    }
}

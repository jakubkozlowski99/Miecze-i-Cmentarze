using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviourExtension
{
    // Start is called before the first frame update
    protected override void Start()
    {
        gameManager.floatingTextManager = FindObjectOfType<FloatingTextManager>();
        audioManager.PlayMusic("theme_" + SceneManager.GetActiveScene().buildIndex);
    }
}

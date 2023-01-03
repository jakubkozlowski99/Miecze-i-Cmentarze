using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.floatingTextManager = FindObjectOfType<FloatingTextManager>();
    }
}

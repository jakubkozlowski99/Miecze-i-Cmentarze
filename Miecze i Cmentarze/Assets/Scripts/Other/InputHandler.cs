using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;
    public KeyBinds keyBinds;


    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // configure key binds
        keyBinds = new KeyBinds();
        keyBinds.ConfigureKeys(); // TO DO: zapisywanie i wczytywanie przyciskow
    }

    public bool CheckKey(string function)
    {
        if (function == "Up" || function == "Down" || function == "Left" || function == "Right")
        {
            if (Input.GetKey(keyBinds.binds[function])) return true;
            else return false;
        }

        if (Input.GetKeyDown(keyBinds.binds[function])) return true;
        else return false;
    }
}
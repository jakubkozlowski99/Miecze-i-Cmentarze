using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds
{
    public Dictionary<string, KeyCode> binds;

    public void ConfigureKeys()
    {
        binds = new Dictionary<string, KeyCode>
        {
            { "Up", KeyCode.W },
            { "Down", KeyCode.S },
            { "Left", KeyCode.A },
            { "Right", KeyCode.D },
            { "Interaction", KeyCode.E },
            { "Dodge", KeyCode.Space },
            { "Attack", KeyCode.Q },
            { "Toggle_Inventory", KeyCode.Tab }
        };
    }
}

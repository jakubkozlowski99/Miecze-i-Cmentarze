using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds
{
    public Dictionary<KeyActions, KeyCode> binds;

    public void ConfigureKeys()
    {
        binds = new Dictionary<KeyActions, KeyCode>
        {
            { KeyActions.Up, KeyCode.W },
            { KeyActions.Down, KeyCode.S },
            { KeyActions.Left, KeyCode.A },
            { KeyActions.Right, KeyCode.D },
            { KeyActions.Interaction, KeyCode.E },
            { KeyActions.Dodge, KeyCode.Space },
            { KeyActions.Attack, KeyCode.Q },
            { KeyActions.Toggle_Inventory, KeyCode.Tab }
        };

        if (SaveManager.instance.tempKeyIntValues != null && SaveManager.instance.tempKeyIntValues.Count > 0) LoadKeys();
    }

    public void LoadKeys()
    {
        int i = 0;
        var newBinds = new Dictionary<KeyActions, KeyCode>();
        foreach (var bind in binds)
        {
            newBinds.Add(bind.Key, (KeyCode)SaveManager.instance.tempKeyIntValues[i]);
            i++;
        }
        binds = newBinds;
    }
}

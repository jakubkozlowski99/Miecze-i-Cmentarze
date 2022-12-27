using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public PlayerData playerData;

    public SaveData()
    {

    }
}

[Serializable]
public class PlayerData
{
    public int level;

    public PlayerData(int level)
    {
        this.level = level;
    }
}

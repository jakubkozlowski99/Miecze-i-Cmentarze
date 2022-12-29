using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Load();
        }
    }

    private void Save()
    {
        Debug.Log("Zapisano stan rozgrywki");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.OpenOrCreate);

        SaveData data = new SaveData();
        SavePlayer(data);

        bf.Serialize(file, data);

        file.Close();
    }

    private void SavePlayer(SaveData data)
    {
        data.playerData = new PlayerData(GameManager.instance.playerLevel);
    }

    private void Load()
    {
        Debug.Log("Wczytano stan rozgrywki");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

        SaveData data = (SaveData)bf.Deserialize(file);
        LoadPlayer(data);

        file.Close();

    }

    private void LoadPlayer(SaveData data)
    {
        GameManager.instance.playerLevel = data.playerData.level;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public List<Item> allItems;

    public List<Enemy> allEnemies;

    public bool isLoading;
    public bool loadTempData;

    //private Chest[] chests;

    public List<ChestData> tempChests;

    public List<QuestData> tempQuests;

    public List<QuestData> tempCompletedQuests;

    public List<ShrineData> tempShrines;

    public List<SpawnerData> tempSpawners;

    public List<BossData> tempBosses;

    public int tempSceneIndex;

    public float tempTimer;

    public float tempMusicVolume;
    public float tempSoundsVolume;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (FileExists(Application.persistentDataPath + "/" + "SaveTest.dat")) LoadTempData();

        LoadGlobals();
    }

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void ResetTemps()
    {
        tempChests = new List<ChestData>();
        tempQuests = new List<QuestData>();
        tempCompletedQuests = new List<QuestData>();
        tempShrines = new List<ShrineData>();
        tempSpawners = new List<SpawnerData>();
        tempBosses = new List<BossData>();
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public void Save()
    {
        Debug.Log("Zapisano stan rozgrywki");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.OpenOrCreate);

        SaveData data = new SaveData();
        data.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.mapsUnlocked = GameManager.instance.mapsUnlocked;
        SavePlayer(data);
        SaveInventory(data);
        data.chestData = tempChests;
        SaveQuests(data);
        data.shrineData = tempShrines;
        data.gameTimer = GameManager.instance.gameTimer;
        SaveSpawners();
        data.spawnerData = tempSpawners;
        data.bossData = tempBosses;

        bf.Serialize(file, data);

        file.Close();
    }

    private void SavePlayer(SaveData data)
    {
        data.playerData = new PlayerData(GameManager.instance.playerLevel, GameManager.instance.coins, GameManager.instance.experience, GameManager.instance.availablePoints,
            GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint, GameManager.instance.player.stamina, GameManager.instance.player.maxStamina,
            new Vector2(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y),
            GameManager.instance.player.playerStats.attack, GameManager.instance.player.playerStats.speed,
            GameManager.instance.player.playerStats.vitality, GameManager.instance.player.playerStats.defense,
            GameManager.instance.player.playerStats.addedAttackPoints, GameManager.instance.player.playerStats.addedSpeedPoints,
            GameManager.instance.player.playerStats.addedVitalityPoints, GameManager.instance.player.playerStats.addedDefensePoints);
        Debug.Log(data.playerData.hp);
    }

    public void Load()
    {
        Debug.Log("Wczytano stan rozgrywki");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

        SaveData data = (SaveData)bf.Deserialize(file);
        LoadPlayer(data);
        LoadInventory(data);
        GameManager.instance.gameTimer = data.gameTimer;
        GameManager.instance.mapsUnlocked = data.mapsUnlocked;

        file.Close();

    }

    public void LoadTempData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

        SaveData data = (SaveData)bf.Deserialize(file);

        tempSceneIndex = data.sceneIndex;
        tempChests = data.chestData;
        tempQuests = data.questData;
        tempCompletedQuests = data.completedQuestData;
        tempShrines = data.shrineData;
        tempSpawners = data.spawnerData;
        tempTimer = data.gameTimer;
        tempBosses = data.bossData;

        file.Close();
    }

    public void LoadPlayer(SaveData data)
    {
        /*BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

        SaveData data = (SaveData)bf.Deserialize(file);*/
        Debug.Log(data.playerData.hp);
        GameManager.instance.playerLevel = data.playerData.level;
        GameManager.instance.coins = data.playerData.coins;
        GameManager.instance.experience = data.playerData.xp;
        GameManager.instance.availablePoints = data.playerData.abilityPoints;
        GameManager.instance.player.healthBar.SetAllBars("hp");
        GameManager.instance.player.staminaBar.SetAllBars("stamina");
        GameManager.instance.player.xpBar.SetXpBar();
        GameManager.instance.player.transform.position = new Vector3(data.playerData.playerX, data.playerData.playerY);
        GameManager.instance.player.playerStats.attack = data.playerData.attack;
        GameManager.instance.player.playerStats.speed = data.playerData.speed;
        GameManager.instance.player.playerStats.vitality = data.playerData.vitality;
        GameManager.instance.player.playerStats.defense = data.playerData.defense;
        GameManager.instance.player.playerStats.addedAttackPoints = data.playerData.addedAttackPoints;
        GameManager.instance.player.playerStats.addedSpeedPoints = data.playerData.addedSpeedPoints;
        GameManager.instance.player.playerStats.addedVitalityPoints = data.playerData.addedVitalityPoints;
        GameManager.instance.player.playerStats.addedDefensePoints = data.playerData.addedDefensePoints;
        GameManager.instance.player.playerStats.basicDamage += data.playerData.addedAttackPoints * 5;
        GameManager.instance.player.playerStats.critChance += data.playerData.addedAttackPoints * 2;
        GameManager.instance.player.playerStats.bonusAttackSpeed += data.playerData.addedSpeedPoints * 5;
        GameManager.instance.player.playerStats.bonusSpeed += data.playerData.addedSpeedPoints * 5;
        GameManager.instance.player.playerStats.bonusStamina += data.playerData.addedSpeedPoints * 20;
        GameManager.instance.player.playerStats.bonusStaminaRegen += data.playerData.addedSpeedPoints * 50;
        GameManager.instance.player.playerStats.bonusHp += data.playerData.addedVitalityPoints * 40;
        GameManager.instance.player.playerStats.damageReduction += data.playerData.addedDefensePoints * 2;

        //file.Close();
    }

    private void SaveInventory(SaveData data)
    {
        //do zmiany
        List<Item> equippedItems = new List<Item>();
        foreach (var equippedItemSlot in InventoryUI.instance.equippedItemSlots)
        {
            if (equippedItemSlot.item != null) equippedItems.Add(equippedItemSlot.item);
        }
        data.inventoryData = new InventoryData(Inventory.instance.items, equippedItems);
    }

    private void LoadInventory(SaveData data)
    {
        foreach (var equippedItemName in data.inventoryData.equippedItemNames)
        {
            foreach (var item in allItems)
            {
                if (item.name == equippedItemName)
                {
                    foreach (var equippedItemSlot in InventoryUI.instance.equippedItemSlots)
                    {
                        if (item.type == equippedItemSlot.itemType)
                        {
                            equippedItemSlot.OnEquip(item);
                        }
                    }
                }
            }
        }
        foreach(var inventoryItemName in data.inventoryData.itemNames)
        {
            foreach (var item in allItems)
            {
                if (item.name == inventoryItemName)
                {
                    Inventory.instance.Add(item);
                }
            }
        }
        GameManager.instance.player.maxhitpoint = data.playerData.maxHP;
        GameManager.instance.player.hitpoint = data.playerData.hp;
        GameManager.instance.player.stamina = data.playerData.stamina;
        GameManager.instance.player.maxStamina = data.playerData.maxStamina;
    }

    private void SaveQuests(SaveData data)
    {
        data.questData = new List<QuestData>();
        data.completedQuestData = new List<QuestData>();
        foreach(Quest quest in GameManager.instance.playerQuests)
        {
            data.questData.Add(new QuestData(quest));
        }
        foreach (Quest completedQuest in GameManager.instance.completedQuests)
        {
            data.completedQuestData.Add(new QuestData(completedQuest));
        }
    }

    /*private void SaveChests(SaveData data)
    {
        data.chestData = tempChests;

        foreach (Chest chest in chests) 
        {
            data.chestData.Add(new ChestData(chest.name, chest.chestItems));
        }
    }*/

    /*private void LoadChests(SaveData data)
    {
        chests = FindObjectsOfType<Chest>();

        foreach (Chest chest in chests) 
        {
            foreach (ChestData chestData in data.chestData) 
            {
                if (chestData.chestName == chest.name)
                {
                    chest.chestItems = new List<Item>();
                    foreach (string itemName in chestData.itemNames)
                    {
                        foreach(Item item in allItems)
                        {
                            if(itemName == item.name)
                            {
                                chest.chestItems.Add(item);
                            }
                        }
                    }
                }
            }
        }

        tempChests = data.chestData;
        Debug.Log("Wczytano stan skrzynek " + tempChests.Count);
    }*/

    public void SaveSpawners()
    {
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.SaveSpawner();
        }
    }

    public void SaveGlobals()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "GlobalSettings.dat", FileMode.Open);

        GlobalData data = new GlobalData();

        tempMusicVolume = AudioManager.instance.musicVolume;
        tempSoundsVolume = AudioManager.instance.soundsVolume;

        data.musicVolume = tempMusicVolume;
        data.soundsVolume = tempSoundsVolume;

        bf.Serialize(file, data);

        file.Close();
    }

    public void LoadGlobals()
    {
        if (FileExists(Application.persistentDataPath + "/" + "GlobalSettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "GlobalSettings.dat", FileMode.Open);

            GlobalData data = (GlobalData)bf.Deserialize(file);
            tempMusicVolume = data.musicVolume;
            tempSoundsVolume = data.soundsVolume;

            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "GlobalSettings.dat", FileMode.Create);

            GlobalData data = new GlobalData();
            tempMusicVolume = 1f;
            tempSoundsVolume = 1f;

            data.musicVolume = tempMusicVolume;
            data.soundsVolume = tempSoundsVolume;

            bf.Serialize(file, data);

            file.Close();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGlobals();
    }
}

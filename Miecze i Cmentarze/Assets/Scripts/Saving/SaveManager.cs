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

    public bool isLoading;

    private Chest[] chests;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Save Manager in the scene");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

    public void Save()
    {
        Debug.Log("Zapisano stan rozgrywki");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.OpenOrCreate);

        SaveData data = new SaveData();
        data.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SavePlayer(data);
        SaveInventory(data);
        SaveChests(data);

        Debug.Log(Inventory.instance.items.Count);

        bf.Serialize(file, data);

        file.Close();
    }

    private void SavePlayer(SaveData data)
    {
        data.playerData = new PlayerData(GameManager.instance.playerLevel, GameManager.instance.coins, GameManager.instance.experience, GameManager.instance.availablePoints,
            GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint, GameManager.instance.player.stamina, GameManager.instance.player.maxStamina,
            new Vector2(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y),
            GameManager.instance.player.playerStats.attack, GameManager.instance.player.playerStats.speed, GameManager.instance.player.playerStats.agility,
            GameManager.instance.player.playerStats.vitality, GameManager.instance.player.playerStats.condition, GameManager.instance.player.playerStats.defense,
            GameManager.instance.player.playerStats.addedAttackPoints, GameManager.instance.player.playerStats.addedSpeedPoints,
            GameManager.instance.player.playerStats.addedAgilityPoints, GameManager.instance.player.playerStats.addedVitalityPoints,
            GameManager.instance.player.playerStats.addedConditionPoints, GameManager.instance.player.playerStats.addedDefensePoints);
    }

    public void Load()
    {
        Debug.Log("Wczytano stan rozgrywki");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

        SaveData data = (SaveData)bf.Deserialize(file);
        Debug.Log("siema");
        LoadPlayer(data);
        LoadInventory(data);
        LoadChests(data);

        file.Close();

    }

    private void LoadPlayer(SaveData data)
    {
        GameManager.instance.playerLevel = data.playerData.level;
        GameManager.instance.coins = data.playerData.coins;
        GameManager.instance.experience = data.playerData.xp;
        GameManager.instance.availablePoints = data.playerData.abilityPoints;
        GameManager.instance.player.hitpoint = data.playerData.hp;
        GameManager.instance.player.maxhitpoint = data.playerData.maxHP;
        GameManager.instance.player.stamina = data.playerData.stamina;
        GameManager.instance.player.maxStamina = data.playerData.maxStamina;
        GameManager.instance.player.healthBar.SetAllBars("hp");
        GameManager.instance.player.staminaBar.SetAllBars("stamina");
        GameManager.instance.player.xpBar.SetXpBar();
        GameManager.instance.player.transform.position = new Vector3(data.playerData.playerX, data.playerData.playerY);
        GameManager.instance.player.playerStats.attack = data.playerData.attack;
        GameManager.instance.player.playerStats.speed = data.playerData.speed;
        GameManager.instance.player.playerStats.agility = data.playerData.agility;
        GameManager.instance.player.playerStats.vitality = data.playerData.vitality;
        GameManager.instance.player.playerStats.condition = data.playerData.condition;
        GameManager.instance.player.playerStats.defense = data.playerData.defense;
        GameManager.instance.player.playerStats.addedAttackPoints = data.playerData.addedAttackPoints;
        GameManager.instance.player.playerStats.addedSpeedPoints = data.playerData.addedSpeedPoints;
        GameManager.instance.player.playerStats.addedAgilityPoints = data.playerData.addedAgilityPoints;
        GameManager.instance.player.playerStats.addedVitalityPoints = data.playerData.addedVitalityPoints;
        GameManager.instance.player.playerStats.addedConditionPoints = data.playerData.addedConditionPoints;
        GameManager.instance.player.playerStats.addedDefensePoints = data.playerData.addedDefensePoints;
    }

    private void SaveInventory(SaveData data)
    {
        data.inventoryData = new InventoryData(Inventory.instance.items, InventoryUI.instance.helmet.item, InventoryUI.instance.weapon.item,
            InventoryUI.instance.armor.item, InventoryUI.instance.ring.item, InventoryUI.instance.boots.item, InventoryUI.instance.gloves.item);
    }

    private void LoadInventory(SaveData data)
    {
        foreach (string itemName in data.inventoryData.itemNames)
        {
            foreach(Item item in allItems)
            {
                if (item.name == itemName) Inventory.instance.Add(item);
            }
        }

        foreach (Item item in allItems)
        {
            if (item.name == data.inventoryData.helmetName) 
            {
                InventoryUI.instance.helmet.OnEquip(item);
            }
            if (item.name == data.inventoryData.weaponName)
            {
                InventoryUI.instance.weapon.OnEquip(item);
            }
            if (item.name == data.inventoryData.armorName)
            {
                InventoryUI.instance.armor.OnEquip(item);
            }
            if (item.name == data.inventoryData.ringName)
            {
                InventoryUI.instance.ring.OnEquip(item);
            }
            if (item.name == data.inventoryData.bootsName)
            {
                InventoryUI.instance.boots.OnEquip(item);
            }
            if (item.name == data.inventoryData.glovesName)
            {
                InventoryUI.instance.gloves.OnEquip(item);
            }
        }
    }

    private void SaveChests(SaveData data)
    {
        chests = FindObjectsOfType<Chest>();
        data.chestData = new List<ChestData>();

        foreach (Chest chest in chests) 
        {
            data.chestData.Add(new ChestData(chest.name, chest.chestItems));
        }
    }

    private void LoadChests(SaveData data)
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        foreach (Quest quest in playerQuests) quest.SetGoal();
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Game Manager in the scene");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable = new List<int>();
    public Player player;

    public FloatingTextManager floatingTextManager;

    public int coins = 0;
    public int experience = 0;

    [SerializeField]
    public int playerLevel = 1;
    public int availablePoints = 0;

    [SerializeField]
    public List<Quest> playerQuests;

    public void Start()
    {
        
    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);

        Debug.Log("LoadState");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;

    [Header("Choices UI")]
    public GameObject[] choices;

    public GameObject ShopUI;

    private TextMeshProUGUI[] choicesText;

    public TextMeshProUGUI dialogueText;

    private Story currentStory;
    public bool dialogueIsPlaying;

    public static DialogueManager instance;

    public Animator portraitAnim;

    private const string ACTION_TAG = "action";

    public ShopUI shopUI;

    private Quest quest;

    public NPC npc;

    Shop shop;

    public QuestsUI questsUI;

    private GameManager gameManager;
    private Player player;
    private Inventory inventory;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        shopUI = FindObjectOfType<ShopUI>();
        ShopUI = FindObjectOfType<ShopUI>().gameObject;
        questsUI = FindObjectOfType<QuestsUI>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        player = GameManager.instance.player;
        inventory = Inventory.instance;

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];

        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
       
        if (currentStory.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, string newPortrait, Shop newShop, Quest newQuest, NPC newNPC)
    {
        quest = newQuest;
        npc = newNPC;
        inventory.canToggle = false;
        currentStory = new Story(inkJSON.text);
        currentStory.variablesState["questState"] = SetQuestState();
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        portraitAnim.SetTrigger(newPortrait);
        Debug.Log(newShop.shopItems.Count);
        shop = newShop;
        player.canMove = false;
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        inventory.canToggle = true;
        portraitAnim.SetTrigger("Exit");
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        player.canMove = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case ACTION_TAG:
                    if (tagValue == "shop") OpenShop();
                    if (tagValue == "quest_ask") 
                    {
                        if (SetQuestState() != 1) dialogueText.text = quest.information.dialogueText;
                    }
                    if (tagValue == "quest_give") GiveQuest();
                    if (tagValue == "quest_reward") CompleteQuest();
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogWarning("Za duzo wybor�w w dialogu");
        }

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void OpenShop()
    {
        dialoguePanel.SetActive(false);
        shopUI.OpenShop(shop);
    }

    private int SetQuestState()
    {
        if (quest == null) return 1;

        foreach (Quest playerQuest in gameManager.playerQuests)
        {
            if (playerQuest == quest)
            {
                if (playerQuest.completed == false)
                {
                    return 2;
                }
                else return 3;
            }
        }
        return 0;
    }

    private void GiveQuest()
    {
        gameManager.playerQuests.Add(quest);
        //quest.SetGoal();
        inventory.CheckQuestItems();
        gameManager.CheckQuestBosses();
        questsUI.AddQuest(quest);
        gameManager.ShowText("Dodano zadanie", 10, Color.yellow, new Vector3(player.transform.position.x,
            player.transform.position.y + 0.5f, transform.position.z), Vector3.up * 25, 0.5f, true) ;
        npc.SetAttentionMark();
    }

    private void CompleteQuest()
    {
        quest.rewardClaimed = true;

        gameManager.completedQuests.Add(quest);
        gameManager.playerQuests.Remove(quest);

        foreach (QuestGoal questGoal in quest.goals)
        {
            if (questGoal.goalType == QuestGoal.GoalType.Gathering)
            {
                for (int i = 0; i < questGoal.requiredAmount; i++) Inventory.instance.Remove(questGoal.itemGoal);
            }
        }

        questsUI.RemoveQuest(quest);

        gameManager.coins += quest.reward.coins;
        gameManager.experience += quest.reward.xp;
        if (gameManager.experience >= gameManager.xpTable[gameManager.playerLevel - 1])
        {
            player.LevelUp();
        }
        player.xpBar.SetXpBar();
        gameManager.ShowText("+" + quest.reward.coins+ " monet", 10, Color.yellow, new Vector3(player.transform.position.x,
            player.transform.position.y + player.textOffset, transform.position.z), Vector3.up * 25, 0.5f, true);
        gameManager.ShowText("+" + quest.reward.xp + "XP", 10, Color.yellow, new Vector3(player.transform.position.x,
            player.transform.position.y + player.textOffset, transform.position.z), Vector3.up * 25, 0.5f, true);

        npc.SetAttentionMark();
    }
}

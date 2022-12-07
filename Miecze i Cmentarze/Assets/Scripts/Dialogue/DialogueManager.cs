using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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

    Shop shop;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Wi?cej ni? jeden DialogueManager");
        }
        instance = this;
    }

    private void Start()
    {
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

    public void EnterDialogueMode(TextAsset inkJSON, string newPortrait, Shop newShop)
    {
        Inventory.instance.canToggle = false;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        portraitAnim.SetTrigger(newPortrait);
        Debug.Log(newShop.shopItems.Count);
        shop = newShop;
        GameManager.instance.player.canMove = false;
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        Inventory.instance.canToggle = true;
        portraitAnim.SetTrigger("Exit");
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        GameManager.instance.player.canMove = true;
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
            Debug.LogWarning("Za duzo wyborów w dialogu");
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

    public void CloseShop()
    {
        dialoguePanel.SetActive(true);
        shopUI.CloseShop();
    }
}

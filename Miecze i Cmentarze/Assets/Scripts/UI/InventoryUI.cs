using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    Inventory inventory;

    public List<GameObject> panelsUI;

    public InventorySlot[] slots;
    public ItemDetailsUI itemDetailsUI;
    public ItemDetailsUI equippedItemDetailsUI;

    public QuestsUI questPanel;

    public GameObject statsPanelUI;

    public StatsPanelUI statsPanel;

    public List<Image> panelTabs;

    public List<Image> tabIcons;

    public Sprite activeTab;
    public Sprite unactiveTab;

    public List<Sprite> activeIcons;
    public List<Sprite> unactiveIcons;

    private int tab;

    public List<EquippedItemSlot> equippedItemSlots;

    public Text abilityPoints;
    public Text level;
    public Text attack;
    public Text speed;
    public Text vitality;
    public Text defense;
    public Text coins;

    public GameObject attackButton;
    public GameObject speedButton;
    public GameObject vitalityButton;
    public GameObject defenseButton;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.canToggle = true;
        //inventory.onItemChangedCallback += UpdateInventory;
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNumber = i;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventory.canToggle && !PauseMenu.instance.gameIsPaused)
        {
            if (panelsUI.TrueForAll(p => !p.activeSelf))
            {
                tab = 0;
                SetTabs(tab);
                UpdateInventory();
                statsPanel.SetStatsText();
                RemoveHighlights();
                foreach (var panelTab in panelTabs)
                {
                    panelTab.gameObject.SetActive(true);
                }
                panelsUI[tab].SetActive(true);
                GameManager.instance.player.canMove = false;
                GameManager.instance.player.canAttack = false;
            }
            else
            {
                foreach (var panelTab in panelTabs) 
                {
                    panelTab.gameObject.SetActive(false);
                }

                GameManager.instance.player.canMove = true;

                if (questPanel.selectedQuest != null) questPanel.selectedQuest.highlightImage.enabled = false;
                questPanel.selectedQuest = null;
                questPanel.ClearDescription();
                itemDetailsUI.HideDetails();
                equippedItemDetailsUI.HideDetails();

                foreach(var panelUI in panelsUI)
                {
                    panelUI.SetActive(false);
                }
                GameManager.instance.player.canAttack = true;
            }
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        abilityPoints.text = GameManager.instance.availablePoints.ToString();
        level.text = GameManager.instance.playerLevel.ToString();
        attack.text = GameManager.instance.player.playerStats.attack.ToString();
        speed.text = GameManager.instance.player.playerStats.speed.ToString();
        vitality.text = GameManager.instance.player.playerStats.vitality.ToString();
        defense.text = GameManager.instance.player.playerStats.defense.ToString();
        coins.text = GameManager.instance.coins.ToString();

        UpdateAddPointButtons();
    }
    
    public void UpdateAddPointButtons()
    {
        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedAttackPoints < 9)
        {
            attackButton.SetActive(true);
        }
        else attackButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedSpeedPoints < 9)
        {
            speedButton.SetActive(true);
        }
        else speedButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedVitalityPoints < 9)
        {
            vitalityButton.SetActive(true);
        }
        else vitalityButton.SetActive(false);

        if (GameManager.instance.availablePoints > 0 && GameManager.instance.player.playerStats.addedDefensePoints < 9)
        {
            defenseButton.SetActive(true);
        }
        else defenseButton.SetActive(false);
    }

    public void RemoveHighlights()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].highlightImage.enabled = false;
            slots[i].highlighted = false;
        }

        foreach (var equippedItemSlot in equippedItemSlots)
        {
            equippedItemSlot.highlightImage.enabled = false;
            equippedItemSlot.highlighted = false;
        }
    }

    private void SetTabs(int tabIndex)
    {
        var index = 0; 
        foreach (var panelTab in panelTabs)
        {
            if (index == tabIndex)
            {
                panelTab.sprite = activeTab;
                tabIcons[index].sprite = activeIcons[index];
            }
            else
            {
                panelTab.sprite = unactiveTab;
                tabIcons[index].sprite = unactiveIcons[index];
            }
            index++;
        }
        tab = tabIndex;
    }

    public void ClickPanelTab(int tabIndex)
    {
        if (tab != tabIndex)
        {
            var index = 0;
            foreach (var panelUI in panelsUI)
            {
                if (index == tabIndex)
                {
                    panelUI.SetActive(true);
                }
                else
                {
                    panelUI.SetActive(false);
                }
                index++;
            }
            SetTabs(tabIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void SetMaxValue(float health)
    {
        slider.maxValue = health;
        slider.value = health;    }

    public void SetValue(float health)
    {
        slider.value = health;
    }

    public void SetText(float value, float maxValue)
    {
        text.text = value.ToString() + "/" + maxValue.ToString();
    }

    public void SetXpBar()
    {
        if(GameManager.instance.playerLevel == 1)
        {
            slider.maxValue = GameManager.instance.xpTable[0];
            slider.value = GameManager.instance.experience;
        }
        else
        {
            slider.maxValue = GameManager.instance.xpTable[GameManager.instance.playerLevel - 1] - GameManager.instance.xpTable[GameManager.instance.playerLevel - 2];
            slider.value = GameManager.instance.experience - GameManager.instance.xpTable[GameManager.instance.playerLevel - 2];
        }
        float percentage = Mathf.Round((slider.value / slider.maxValue) * 100);
        text.text = percentage.ToString() + "%";
    }

    public void SetAllBars(string barType)
    {
        float value = 0;
        float maxValue = 0;
        if (barType == "hp")
        {
            value = GameManager.instance.player.hitpoint;
            maxValue = GameManager.instance.player.maxhitpoint;        }
        else if (barType == "stamina")
        {
            value = GameManager.instance.player.stamina;
            maxValue = GameManager.instance.player.maxStamina;
        }
        SetMaxValue(maxValue);
        SetValue(value);
        SetText(value, maxValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void setMaxValue(float health)
    {
        slider.maxValue = health;
        slider.value = health;    }

    public void setValue(float health)
    {
        slider.value = health;
    }

    public void setText(float value, float maxValue)
    {
        text.text = value.ToString() + "/" + maxValue.ToString();
    }

    public void setXpBar()
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
}

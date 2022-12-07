using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float attack;
    public float speed;
    public float agility;
    public float vitality;
    public float condition;
    public float defense;

    public int addedAttackPoints;
    public int addedSpeedPoints;
    public int addedAgilityPoints;
    public int addedVitalityPoints;
    public int addedConditionPoints;
    public int addedDefensePoints;


    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.instance.coins += 10005;
            defense++;
            attack++;
            speed++;
            vitality++;
            condition++;
            agility++;
            GameManager.instance.player.playerXSpeed = 2 + 0.1f * speed - 0.1f;
            GameManager.instance.player.playerYSpeed = 1.5f + 0.075f * speed - 0.075f;
            GameManager.instance.player.anim.SetFloat("AttackSpeed", 1 + (condition/10) - 0.1f);
            GameManager.instance.player.maxhitpoint = 60 + (vitality * 40);
            GameManager.instance.player.healthBar.setMaxValue(GameManager.instance.player.maxhitpoint);
            GameManager.instance.player.healthBar.setValue(GameManager.instance.player.hitpoint);
            GameManager.instance.player.healthBar.setText(GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint);
            GameManager.instance.player.maxStamina = 60 + (Mathf.CeilToInt(condition) * 40);
            GameManager.instance.player.staminaBar.setMaxValue(GameManager.instance.player.maxStamina);
            GameManager.instance.player.staminaBar.setValue(GameManager.instance.player.stamina);
            GameManager.instance.player.staminaBar.setText(GameManager.instance.player.stamina, GameManager.instance.player.maxStamina);
        }
    }

    public void AddPoint(string stat)
    {
        if (stat == "Attack")
        {
            attack++;
            addedAttackPoints++;
        }
        else if (stat == "Speed")
        {
            speed++;
            addedSpeedPoints++;
        }
        else if (stat == "Agility")
        {
            agility++;
            addedAgilityPoints++;
        }
        else if (stat == "Vitality")
        {
            vitality++;
            addedVitalityPoints++;
        }
        else if (stat == "Condition")
        {
            condition++;
            addedConditionPoints++;
        }
        else if (stat == "Defense")
        {
            defense++;
            addedDefensePoints++;
        }
        UpdateStats();
    }

    public void UpdateStats()
    {
        GameManager.instance.player.playerXSpeed = 2 + 0.1f * speed - 0.1f;
        GameManager.instance.player.playerYSpeed = 1.5f + 0.075f * speed - 0.075f;

        GameManager.instance.player.anim.SetFloat("AttackSpeed", 1 + (agility / 10) - 0.1f);

        GameManager.instance.player.maxhitpoint = 60 + (vitality * 40);
        GameManager.instance.player.healthBar.setMaxValue(GameManager.instance.player.maxhitpoint);
        if (GameManager.instance.player.hitpoint > GameManager.instance.player.maxhitpoint)
        { 
            GameManager.instance.player.hitpoint = GameManager.instance.player.maxhitpoint; 
        }
        GameManager.instance.player.healthBar.setValue(GameManager.instance.player.hitpoint);
        GameManager.instance.player.healthBar.setText(GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint);

        GameManager.instance.player.maxStamina = 60 + (Mathf.CeilToInt(condition) * 40);
        GameManager.instance.player.staminaBar.setMaxValue(GameManager.instance.player.maxStamina);
        if (GameManager.instance.player.stamina > GameManager.instance.player.maxStamina)
        {
            GameManager.instance.player.stamina = GameManager.instance.player.maxStamina;
        }
        GameManager.instance.player.staminaBar.setValue(GameManager.instance.player.stamina);
        GameManager.instance.player.staminaBar.setText(GameManager.instance.player.stamina, GameManager.instance.player.maxStamina);
    }
}

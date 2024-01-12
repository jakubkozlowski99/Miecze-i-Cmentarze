using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float attack;
    public float speed;
    public float vitality;
    public float defense;

    public float basicDamage;
    public float armorPenetration;
    public float bonusAttackSpeed;
    public float critChance;

    public float bonusHp;
    public float bonusHpRegen;

    public float bonusSpeed;
    public float bonusStamina;
    public float bonusStaminaRegen;
    public bool freeDodge = false;

    public float damageReduction;

    public int addedAttackPoints;
    public int addedSpeedPoints;
    public int addedVitalityPoints;
    public int addedDefensePoints;

    public float hpPercentage;

    public void AddPoint(string stat)
    {
        //adding player stat based on stat name
        if (stat == "Attack")
        {
            attack++;
            basicDamage += 5;
            critChance += 2;
            addedAttackPoints++;
        }
        else if (stat == "Speed")
        {
            speed++;
            bonusAttackSpeed += 5;
            bonusSpeed += 5;
            bonusStamina += 20;
            bonusStaminaRegen += 50;
            addedSpeedPoints++;
        }
        else if (stat == "Vitality")
        {
            vitality++;
            bonusHp += 40;
            addedVitalityPoints++;
        }
        else if (stat == "Defense")
        {
            defense++;
            damageReduction += 2;
            addedDefensePoints++;
        }

        //updating player stats
        UpdateStats();
    }

    public void UpdateStats()
    {
        //setting player attributes by stat values
        hpPercentage = GameManager.instance.player.hitpoint / GameManager.instance.player.maxhitpoint;
        hpPercentage = Mathf.Round((GameManager.instance.player.hitpoint / GameManager.instance.player.maxhitpoint) * 100) / 100;
        GameManager.instance.player.playerSpeed = 2 + 0.01f * bonusSpeed;

        GameManager.instance.player.anim.SetFloat("AttackSpeed", 1 + (bonusAttackSpeed / 100) - 0.1f);

        GameManager.instance.player.maxhitpoint = 100 + bonusHp;
        GameManager.instance.player.healthBar.SetMaxValue(GameManager.instance.player.maxhitpoint);
        /*if (GameManager.instance.player.hitpoint > GameManager.instance.player.maxhitpoint)
        { 
            GameManager.instance.player.hitpoint = GameManager.instance.player.maxhitpoint; 
        }*/
        GameManager.instance.player.hitpoint = Mathf.Round(GameManager.instance.player.maxhitpoint * hpPercentage);
        GameManager.instance.player.healthBar.SetValue(GameManager.instance.player.hitpoint);
        GameManager.instance.player.healthBar.SetText(GameManager.instance.player.hitpoint, GameManager.instance.player.maxhitpoint);

        GameManager.instance.player.maxStamina = 100 + (int)bonusStamina;
        GameManager.instance.player.staminaBar.SetMaxValue(GameManager.instance.player.maxStamina);
        if (GameManager.instance.player.stamina > GameManager.instance.player.maxStamina)
        {
            GameManager.instance.player.stamina = GameManager.instance.player.maxStamina;
        }
        GameManager.instance.player.staminaBar.SetValue(GameManager.instance.player.stamina);
        GameManager.instance.player.staminaBar.SetText(GameManager.instance.player.stamina, GameManager.instance.player.maxStamina);

        //setting stat panel text
        InventoryUI.instance.statsPanel.SetStatsText();
    }
}

using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Item")]
[Serializable]
public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;

    public float basicDamage;
    public float armorPenetration;
    public float bonusAttackSpeed;
    public float critChance;
    public float bonusHp;
    public float bonusHpRegen;
    public float bonusSpeed;
    public float bonusStamina;
    public float bonusStaminaRegen;
    public float damageReduction;

    public string type = null; //weapon, armor, boots, helmet, gloves, ring, consumable, questItem
    public int coins = 0;
    public float potionHp = 0;
    public int buyPrice = 0;
    public int sellPrice = 0;
}

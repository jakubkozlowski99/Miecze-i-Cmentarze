using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public int attack = 0;
    public int speed = 0;
    public int agility = 0;
    public int vitality = 0;
    public int condition = 0;
    public int defense = 0;
    public string type = null; //weapon, armor, boots, helmet, gloves, ring, consumable, questItem
    public int coins = 0;
    public float potionHp = 0;
}

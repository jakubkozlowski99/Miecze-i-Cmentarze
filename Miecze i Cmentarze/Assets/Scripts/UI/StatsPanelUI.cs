using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsPanelUI : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    public void SetStatsText()
    {
        statsText.text = "";
        statsText.text += "Zadawane obrażenia: " + GameManager.instance.player.playerStats.basicDamage + "\n";
        statsText.text += "Penetracja pancerza: " + GameManager.instance.player.playerStats.armorPenetration + "%\n";
        statsText.text += "Bonusowa szybkość ataku: " + GameManager.instance.player.playerStats.bonusAttackSpeed + "%\n";
        statsText.text += "Szansa na cios krytyczny: " + GameManager.instance.player.playerStats.critChance + "%\n";
        statsText.text += "Bonusowe punkty zdrowia: " + GameManager.instance.player.playerStats.bonusHp + "\n";
        statsText.text += "Regeneracja zdrowia: " + GameManager.instance.player.playerStats.bonusHpRegen + "/5s\n";
        statsText.text += "Bonusowa szybkość ruchu: " + GameManager.instance.player.playerStats.bonusSpeed + "%\n";
        statsText.text += "Bonusowe punkty kondycji: " + GameManager.instance.player.playerStats.bonusStamina + "\n";
        statsText.text += "Bonusowa regeneracja kondycji: " + GameManager.instance.player.playerStats.bonusStaminaRegen + "%\n";
        statsText.text += "Redukcja obrażeń: " + GameManager.instance.player.playerStats.damageReduction + "%";
    }
}

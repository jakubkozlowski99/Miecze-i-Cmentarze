using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    public List<FloatingText> floatingTexts = new List<FloatingText>();

    private bool onPlayerIsBusy;

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
        {
            if (txt.onPlayer)
            {
                onPlayerIsBusy = floatingTexts.Exists(t => t.onPlayer && t.active);
                if (!onPlayerIsBusy) txt.Show();
            }

            txt.UpdateFloatingText(onPlayerIsBusy);
        }
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration, bool onPlayer)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;

        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.onPlayer = onPlayer;

        if (onPlayer) floatingText.go.SetActive(false);
        else floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active && !t.onPlayer);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }
}

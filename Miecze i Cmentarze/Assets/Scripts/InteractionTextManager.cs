using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<InteractionText> interactionTexts = new List<InteractionText>();

    private void Update()
    {
        foreach (InteractionText txt in interactionTexts)
            txt.UpdateInteractionText();
    }
    public void Show(string msg, int fontSize, Color color, Vector3 position, float offset)
    {
        InteractionText interactionText = GetInteractionText();

        interactionText.txt.text = msg;
        interactionText.txt.fontSize = fontSize;
        interactionText.txt.color = color;
        interactionText.offset = offset;

        interactionText.go.transform.position = Camera.main.ScreenToWorldPoint(position);

        interactionText.Show();
    }

    public void Hide()
    {
        foreach (InteractionText txt in interactionTexts)
            txt.Hide();
    }

    private InteractionText GetInteractionText()
    {
        InteractionText txt = interactionTexts.Find(t => !t.active);

        if(txt == null)
        {
            txt = new InteractionText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            interactionTexts.Add(txt);
        }

        return txt;
    }
}

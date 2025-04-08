using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlSettingsKeyBinding : MonoBehaviour
{
    public KeyActions action;
    public TextMeshProUGUI keyText;
    public ControlSettings controlSettings;

    public bool isWaitingForInput;
    private float placeholderBlinkTimer = 0.75f;
    private float lastBlink;

    private void Update()
    {
        HandlePlaceholderBlinking();

        HandleBindingChangeInput();
    }

    public void SetKeyBindingText()
    {
        keyText.text = InputHandler.instance.keyBinds.binds[action].ToString();
    }

    public void OnClick()
    {
        controlSettings.ResetAllKeyBindingButtons();

        controlSettings.ChangeAllButtonsState();

        isWaitingForInput = true;
        keyText.text = "";
        lastBlink = Time.time;
    }


    public void ResetKeyBindingButton()
    {
        isWaitingForInput = false;
        keyText.text = InputHandler.instance.keyBinds.binds[action].ToString();
        if (keyText.text == "None") keyText.text = "";
    }


    private void HandleBindingChangeInput()
    {
        if (isWaitingForInput)
        {
            foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vkey))
                {
                    if (vkey != KeyCode.Escape)
                    {
                        CheckIfKeyBindingExists(vkey);
                        InputHandler.instance.keyBinds.binds[action] = vkey;
                        controlSettings.ResetAllKeyBindingButtons();
                        controlSettings.ChangeAllButtonsState();
                        return;
                    }
                    else
                    {
                        controlSettings.ResetAllKeyBindingButtons();
                        controlSettings.ChangeAllButtonsState();
                        return;
                    }
                }
            }
        }
    }

    private void CheckIfKeyBindingExists(KeyCode key)
    {
        foreach (var bindingKey in controlSettings.bindingKeys)
        {
            if (InputHandler.instance.keyBinds.binds[bindingKey.action] == key && bindingKey != this) 
                InputHandler.instance.keyBinds.binds[bindingKey.action] = KeyCode.None;
        }
    }

    private void HandlePlaceholderBlinking()
    {
        if (isWaitingForInput == true && Time.time - lastBlink > placeholderBlinkTimer)
        {
            lastBlink = Time.time;
            if (keyText.text.Length > 0) keyText.text = "";
            else keyText.text = ">  <";
        }
    }

}

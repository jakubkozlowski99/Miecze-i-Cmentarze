using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour
{
    public List<ControlSettingsKeyBinding> bindingKeys;
    private InputHandler inputHandler;

    private void OnEnable()
    {
        inputHandler = InputHandler.instance;

        foreach (var bindingKey in bindingKeys)
        {
            bindingKey.ResetKeyBindingButton();
        }

        Debug.Log("Control settings panel enabled");
    }

    private void OnDisable()
    {
        foreach (var bindingKey in bindingKeys) bindingKey.isWaitingForInput = false;
    }


    public void ResetAllKeyBindingButtons()
    {
        foreach (var bindingKey in bindingKeys) bindingKey.ResetKeyBindingButton();
    }


    public void ChangeAllButtonsState()
    {
        foreach (var bindingKey in bindingKeys)
        {
            Button btn = bindingKey.GetComponent<Button>();
            btn.enabled = !btn.enabled;
        }
        var backBtn = GameObject.Find("BackButtonControlSettings");
        backBtn.GetComponent<Button>().enabled = !backBtn.GetComponent<Button>().enabled;
    }
}

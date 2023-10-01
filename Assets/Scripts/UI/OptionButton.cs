using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    public GameObject optionPanel;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        RAMManager.Instance.onRAMModeEnabled += OnRAMMode;
    }

    private void OnRAMMode(bool isOn)
    {
        optionPanel.SetActive(false);
    }

    private void OnClick()
    {
        SFXManager.PlaySound(GlobalSFX.ButtonClick);
        if (RAMManager.Instance.isInRAMMode)
            return;
        optionPanel.SetActive(!optionPanel.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRAMCount : MonoBehaviour
{
    TextMeshProUGUI text;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        RAMManager.Instance.onRAMCountChanged += OnRAMCountChanged;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnRAMCountChanged(int newRAMCount)
    {
        text.text = $":0x0{newRAMCount}";

        button.interactable = newRAMCount > 0;
    }


}

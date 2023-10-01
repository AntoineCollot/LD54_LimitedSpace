using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRAMTuto : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        RAMManager.Instance.onRAMCountChanged += OnRAMCountChanged;
    }

    private void OnRAMCountChanged(int newRAMCount)
    {
        if(newRAMCount>0)
        {
            target.SetActive(true);
            RAMManager.Instance.onRAMCountChanged -= OnRAMCountChanged;
        }
    }
}

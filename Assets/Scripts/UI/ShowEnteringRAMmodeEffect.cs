using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEnteringRAMmodeEffect : MonoBehaviour
{
    public GameObject effect;

    private void Start()
    {
        RAMManager.Instance.onRAMModeEnabled += OnRAMPModeEnter;
        effect.SetActive(false);
    }

    private void OnDestroy()
    {
        if(RAMManager.Instance != null)
            RAMManager.Instance.onRAMModeEnabled -= OnRAMPModeEnter;
    }

    private void OnRAMPModeEnter(bool value)
    {
        effect.SetActive(value);
    }
}

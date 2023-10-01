using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public GameObject foreground;
    public Toggle muteMusicToggle;
    public Toggle muteSFXToggle;
    public Toggle translateToggle;

    private void OnEnable()
    {
        foreground.SetActive(RAMManager.Instance.GetState(RAMType.Options) != RAMState.Unlocked);
    }

    private void Start()
    {
        muteMusicToggle.isOn = SFXManager.muteMusic;
        muteSFXToggle.isOn = SFXManager.muteSFX;
        translateToggle.isOn = Ally.translateToEnglish;

        muteMusicToggle.onValueChanged.AddListener(OnMusicChanged);
        muteSFXToggle.onValueChanged.AddListener(OnSFXChanged);
        translateToggle.onValueChanged.AddListener(OnTranslateChanged);
    }

    private void OnTranslateChanged(bool arg0)
    {
        Ally.translateToEnglish = arg0;
    }

    private void OnMusicChanged(bool arg0)
    {
        SFXManager.MuteMusic(arg0);
    }

    private void OnSFXChanged(bool arg0)
    {
        SFXManager.MuteSFX(arg0);
    }


}

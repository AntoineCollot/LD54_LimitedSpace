using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideSpeachPanel : MonoBehaviour
{
    public RAMType RAMType;
    Graphic graphic;

    // Start is called before the first frame update
    void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    private void Start()
    {
        RAMManager.Instance.onRAMStateChanged += OnRAMStateChanged;
    }

    private void OnEnable()
    {
        OnRAMStateChanged(RAMType, RAMManager.Instance.GetState(RAMType));
    }

    private void OnRAMStateChanged(RAMType type, RAMState state)
    {
        if(type == RAMType)
        {
            graphic.enabled = state != RAMState.Unlocked;
        }
    }

    private void OnDestroy()
    {
        if (RAMManager.Instance != null)
            RAMManager.Instance.onRAMStateChanged -= OnRAMStateChanged;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAMInteractable : MonoBehaviour
{
    public RAMType RAMType;
    protected Collider2D col2D;

    protected virtual void Start()
    {
        col2D = GetComponent<Collider2D>();
        if (col2D != null)
            col2D.enabled = false;
        RAMManager.Instance.onRAMModeEnabled += OnRAMMode;
    }

    protected virtual void OnDestroy()
    {
        if (RAMManager.Instance != null)
            RAMManager.Instance.onRAMModeEnabled -= OnRAMMode;
    }

    private void OnRAMMode(bool isOn)
    {
        if (col2D != null)
            col2D.enabled = isOn;
    }
}

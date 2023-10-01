using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAMInteractableInstancedMat : RAMInteractable
{
    public List<Renderer> targetRenderers = new List<Renderer>();

    protected override void Start()
    {
        base.Start();

        RAMManager.Instance.onRAMStateChanged += OnRAMStateChanged;

        OnRAMStateChanged(RAMType, RAMManager.Instance.GetState(RAMType));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(RAMManager.Instance != null)
            RAMManager.Instance.onRAMStateChanged -= OnRAMStateChanged;
    }

    private void OnRAMStateChanged(RAMType type, RAMState state)
    {
        //if (!RAMManager.Instance.isInRAMMode)
        //    return;

        if (type != RAMType)
            return;

        foreach (Renderer renderer in targetRenderers)
        {
            renderer.material.SetInteger("_RAMState", (int)state);
        }
    }
}

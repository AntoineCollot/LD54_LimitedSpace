using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RAMManager : MonoBehaviour
{
    public delegate void OnRAMGroupEventHandler(RAMType type, RAMState state);
    public event OnRAMGroupEventHandler onRAMStateChanged;

    public ScriptableRAMData ramData;

    public void SetState(RAMType type, RAMState state, bool allowDelocking = false)
    {
        RAMGroup group = ramData.ramGroups.First(r => r.type == type);

        SetState(group, state, allowDelocking);
    }

    public void SetState(RAMGroup group, RAMState state, bool allowDelocking = false)
    {
        //Don't do anything to unlocked state
        if (!allowDelocking && group.state == RAMState.Unlocked)
            return;

        group.SetState(state);

        onRAMStateChanged.Invoke(group.type, state);
    }

#if UNITY_EDITOR
    [ContextMenu("TriggerEventForAll")]
    public void DebugTriggerEventForAll()
    {
        foreach (RAMGroup group in ramData.ramGroups)
        {
            onRAMStateChanged.Invoke(group.type, group.state);
        }
    }
#endif
}

[System.Serializable]
public class RAMGroup
{
    public RAMType type;
    public RAMState state { get; private set; }
    public List<Material> materials;

    public void SetState(RAMState state)
    {
        this.state = state;
        if (materials != null)
        {
            foreach (Material material in materials)
            {
                material.SetInteger("_RAMState", (int)state);
            }
        }
    }
}

public enum RAMType
{
    Tilemap,
    Character,
    Enemy0,
    Enemy1,
    Enemy2,
    NPC,
    DigitBlock,
    FX,
}

public enum RAMState { Locked, Hovered, Unlocked }
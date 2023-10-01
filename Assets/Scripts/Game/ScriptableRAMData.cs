using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "NewRAMData", menuName = "ScriptableObjects/RAMData", order = 1)]
public class ScriptableRAMData : ScriptableObject
{
    public int availableRAMCount;
    public RAMGroup[] ramGroups;

    public RAMGroup GetRAMGroup(RAMType type)
    {
        return ramGroups.First(r => r.type == type);
    }

    [ContextMenu("Reset All")]
    public void ResetALl()
    {
        availableRAMCount = 0;
        foreach (var ramGroup in ramGroups)
        {
            ramGroup.SetState(RAMState.Locked,true);
        }
    }
}

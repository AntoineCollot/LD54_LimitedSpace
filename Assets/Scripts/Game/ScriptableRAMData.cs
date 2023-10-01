using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "NewRAMData", menuName = "ScriptableObjects/RAMData", order = 1)]
public class ScriptableRAMData : ScriptableObject
{
    public RAMGroup[] ramGroups;

    public RAMGroup GetRAMGroup(RAMType type)
    {
        return ramGroups.First(r => r.type == type);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKillTargets : MonoBehaviour
{
    public List<Health> targets;
    public ScriptableBool output;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Health health in targets)
        {
            health.onDie.AddListener(OnTargetDied);
        }
    }

    private void OnTargetDied()
    {
        output.value = CheckCompleded();
    }

    bool CheckCompleded()
    {
        foreach (Health health in targets)
        {
            if (!health.isDead)
                return false;
        }

        return true;
    }
}

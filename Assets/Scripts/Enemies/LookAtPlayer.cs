using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponentInParent<EnemyAI>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (!ai.isTracking)
            return;
        float angleToPlayer = Vector2.SignedAngle(PlayerState.Instance.CenterOfMass - transform.position, Vector2.right);
        transform.rotation = Quaternion.Euler(0, 0, -angleToPlayer);
    }
}

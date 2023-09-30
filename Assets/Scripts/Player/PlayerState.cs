using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] Transform centerOfMassTransform;
    public Vector3 CenterOfMass => centerOfMassTransform.position;
    public static PlayerState Instance;

    private void Awake()
    {
        Instance = this;
    }
}

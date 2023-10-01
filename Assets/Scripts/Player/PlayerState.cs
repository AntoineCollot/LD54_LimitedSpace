using System;
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

    private void Start()
    {
        GetComponent<Health>().onDie.AddListener(OnDie);
        GameManager.Instance.onGameOver.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        GetComponent<Health>().Die();
    }

    private void OnDie()
    {
        GameManager.Instance.GameOver();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTargetPos : MonoBehaviour
{
    public float distance = 2;

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = PlayerState.Instance.CenterOfMass;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(playerPos);
        Vector2 direction = (mousePos - playerScreenPos).normalized;
        transform.position = playerPos + direction * distance;
    }
}

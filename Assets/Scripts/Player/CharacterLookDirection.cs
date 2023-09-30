using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLookDirection : MonoBehaviour
{
    public Transform pivot;
    public float lookAngle { get; private set; }
    public Direction LookDirectionVertical
    {
        get
        {
            if (lookAngle > 0)
                return Direction.Up;
            else
                return Direction.Down;
        }
    }

    public Direction LookDirectionHorizontal
    {
        get
        {
            if (Mathf.Abs(lookAngle) < 90)
                return Direction.Right;
            else
                return Direction.Left;
        }
    }


    public static CharacterLookDirection Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCursor();
    }

    void LookAtCursor()
    {
        //Cursor pos
        Vector2 mousePos = Mouse.current.position.ReadValue();

        //Object pos
        Vector2 pivotScreenPos = Camera.main.WorldToScreenPoint(pivot.position);

        //Angle
        lookAngle = Vector2.SignedAngle(Vector2.right, mousePos - pivotScreenPos);
    }
}

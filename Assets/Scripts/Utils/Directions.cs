using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public enum Direction { Up, Right, Left, Down }

public static class DirectionExtensions
{
    public static Direction ToDirection(this Vector2 vector)
    {
        Direction dir;
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y) + 0.05f)
        {
            if (vector.x < 0)
                dir = Direction.Left;
            else
                dir = Direction.Right;
        }
        else
        {
            //Make down the default
            if (vector.y <= 0.05f)
                dir = Direction.Down;
            else
                dir = Direction.Up;
        }
        return dir;
    }

    public static Vector2 ToVector2(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                return Vector2.left;
            case Direction.Right:
                return Vector2.right;
            case Direction.Up:
            default:
                return Vector2.up;
            case Direction.Down:
                return Vector2.down;
        }
    }
}
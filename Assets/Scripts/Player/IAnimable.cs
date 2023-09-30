using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimable
{
    Direction AnimationDirection { get; }
    float AnimationMoveSpeed { get; }
}

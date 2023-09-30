using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(Vector2 direction, float amount);
}

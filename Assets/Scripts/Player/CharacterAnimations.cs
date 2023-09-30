using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    Animator anim;
    int directionHash;
    int moveSpeedHash;
    SpriteRenderer spriteRenderer;
    IAnimable animable;

    bool flipX;
    float direction;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 1f / 6f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponentInParent<Health>().onDie.AddListener(OnDie);
        animable = GetComponentInParent<IAnimable>();
        directionHash = Animator.StringToHash("Direction");
        moveSpeedHash = Animator.StringToHash("MoveSpeed");
    }

    private void OnDie()
    {
        anim.SetBool("IsDead", true);
    }

    void LateUpdate()
    {
        switch (animable.AnimationDirection)
        {
            case Direction.Up:
                direction = 1;
                break;
            case Direction.Right:
            default:
                flipX = false;
                break;
            case Direction.Left:
                flipX = true;
                break;
            case Direction.Down:
                direction = 0;
                break;
        }
        anim.SetFloat(directionHash, direction);
        spriteRenderer.flipX = flipX;

        anim.SetFloat(moveSpeedHash, animable.AnimationMoveSpeed);
    }
}

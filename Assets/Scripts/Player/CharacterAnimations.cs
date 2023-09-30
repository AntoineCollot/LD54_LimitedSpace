using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        //DirectionByMovement();
        DirectionByCursor();

        anim.SetFloat(directionHash, direction);
        spriteRenderer.flipX = flipX;
        anim.SetFloat(moveSpeedHash, animable.AnimationMoveSpeed);
    }

    void DirectionByCursor()
    {
        switch (CharacterLookDirection.Instance.LookDirectionVertical)
        {
            case Direction.Up:
                direction = 1;
                break;
            case Direction.Down:
            default:
                direction = 0;
                break;
        }

        switch (animable.AnimationDirection)
        {
            case Direction.Right:
            default:
                flipX = false;
                break;
            case Direction.Left:
                flipX = true;
                break;
        }
    }

    void DirectionByMovement()
    {
        switch (animable.AnimationDirection)
        {
            case Direction.Up:
                direction = 1;
                break;
            case Direction.Right:
            default:
                //Inverse direction if we should flip
                if (flipX)
                    direction = 1 - direction;
                flipX = false;
                break;
            case Direction.Left:
                //Inverse direction if we should flip
                if (!flipX)
                    direction = 1 - direction;
                flipX = true;
                break;
            case Direction.Down:
                direction = 0;
                break;
        }
    }
}

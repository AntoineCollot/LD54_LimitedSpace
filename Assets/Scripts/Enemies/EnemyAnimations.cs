using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    Animator anim;
    bool isWalkingAnimState;
    EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        enemyAI.onAttack.AddListener(OnAttack);
        anim = GetComponent<Animator>();
        anim.speed = 1f / 6f;

        GetComponentInParent<Health>().onDie.AddListener(OnDie);
    }

    private void OnDie()
    {
        anim.SetBool("IsDead", true);
        ExplosionProvider.Instance.SpawnEnemyDeath(transform.position);
        GetComponent<SpriteRenderer>().sortingOrder = -10;
    }

    private void OnAttack()
    {
        anim.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        //Walking anim
        if (enemyAI.IsWalking != isWalkingAnimState)
        {
            isWalkingAnimState = enemyAI.IsWalking;
            anim.SetBool("IsWalking", isWalkingAnimState);
        }
    }
}

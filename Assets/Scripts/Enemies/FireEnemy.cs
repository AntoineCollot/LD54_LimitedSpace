using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : EnemyAI
{
    [Header("Fire")]
    public Projectile projectilePrefab;
    public Transform fireOrigin;

    protected override void CustomAttack()
    {
        StartCoroutine(AttackFire());
    }

    IEnumerator AttackFire()
    {
        isAttacking = true;
        onAttack.Invoke();

        yield return new WaitForSeconds(attackAnticipationDuration);

        Projectile newProjectile = Instantiate(projectilePrefab, null);
        newProjectile.transform.position = fireOrigin.position;
        newProjectile.Fire(gameObject, PlayerState.Instance.CenterOfMass - fireOrigin.position, LayerMask.NameToLayer("Enemies"));

        yield return new WaitForSeconds(attackDuration - attackAnticipationDuration);

        isAttacking = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health;
    Material instancedMaterial;
    public bool isDead { get; private set; }
    public bool unkillable = false;
    public CompositeState isInvicibleState = new CompositeState();

    public bool applyOnHitFlashes = true;
    public UnityEvent onDie = new UnityEvent();
    public UnityEvent onHit = new UnityEvent();
    public UnityEvent onHeal = new UnityEvent();

    private void Start()
    {
        if (applyOnHitFlashes)
            instancedMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    public void Hit(int damages)
    {
        if (isDead)
            return;

        if (isInvicibleState.IsOn)
            return;

        if(applyOnHitFlashes)
            instancedMaterial.SetFloat("_HitTime", Time.time);
        health -= damages;
        onHit.Invoke();

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        if (unkillable)
            return;

        GetComponentInChildren<Collider2D>().enabled = false;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.isKinematic = true;
        isDead = true;
        onDie.Invoke();
    }

    public void Heal(int amount)
    {
        health += amount;
        onHeal.Invoke();
    }
}

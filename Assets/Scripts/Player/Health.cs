using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health;
    Material instancedMaterial;
    public bool isDead { get; private set; }
    public CompositeState isInvicibleState = new CompositeState();

    public UnityEvent onDie = new UnityEvent();
    public UnityEvent onHit = new UnityEvent();
    public UnityEvent onHeal = new UnityEvent();

    private void Start()
    {
        instancedMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    public void Hit(int damages)
    {
        if (isDead)
            return;

        if (isInvicibleState.IsOn)
            return;

        instancedMaterial.SetFloat("_HitTime", Time.time);
        health -= damages;
        onHit.Invoke();

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        GetComponentInChildren<Collider2D>().enabled = false;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.isKinematic = true;
        isDead = true;
        onDie.Invoke();

       // Destroy(gameObject, 3);
    }

    public void Heal(int amount)
    {
        health += amount;
        onHeal.Invoke();
    }
}

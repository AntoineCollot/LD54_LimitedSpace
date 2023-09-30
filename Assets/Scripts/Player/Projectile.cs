using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float emitSpeed;
    GameObject owner;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    public void Fire(GameObject owner, Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * emitSpeed;
        this.owner = owner;

        transform.rotation = Quaternion.Euler(0, 0, CharacterLookDirection.Instance.lookAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Do not trigger with owner
        if (collision.attachedRigidbody != null && collision.attachedRigidbody.gameObject == owner)
            return;

        //Find the root object
        GameObject target = collision.transform.gameObject;
        if (collision.attachedRigidbody != null)
        {
            target = collision.attachedRigidbody.gameObject;
        }

        //If it's damageable
        if (target.TryGetComponent(out Health health))
        {
            health.Hit(1);
        }

        ExplosionProvider.Instance.SpawnSingleExplosion(transform.position);
        //Destroy the projectile
        Destroy(gameObject);
    }
}

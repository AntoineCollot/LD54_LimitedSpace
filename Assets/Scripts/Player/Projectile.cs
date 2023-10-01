using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float emitSpeed;
    GameObject owner;
    const float EXPLOSION_RADIUS = 0.2f;
    int ignoreLayer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void Fire(GameObject owner, Vector2 direction, int ignoreLayer = -1)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * emitSpeed;
        this.owner = owner;
        this.ignoreLayer = ignoreLayer;

        transform.rotation = Quaternion.Euler(0, 0, CharacterLookDirection.Instance.lookAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Do not trigger with owner
        if (collision.attachedRigidbody != null && collision.attachedRigidbody.gameObject == owner)
            return;

        if (collision.gameObject.layer == ignoreLayer)
            return;

        //Do not collide with triggers
        if (collision.isTrigger)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        //Find the root object
        foreach (Collider2D collider in hits)
        {
            GameObject target = collider.transform.gameObject;
            if (collider.attachedRigidbody != null)
            {
                target = collider.attachedRigidbody.gameObject;
                if (target == owner)
                    continue;
            }

            //If it's damageable
            if (target.TryGetComponent(out Health health))
            {
                health.Hit(1);
            }
        }

        ExplosionProvider.Instance.SpawnSingleExplosion(transform.position);
        SFXManager.PlaySound(GlobalSFX.ProjectileHit);
        //Destroy the projectile
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, EXPLOSION_RADIUS);
    }
#endif
}

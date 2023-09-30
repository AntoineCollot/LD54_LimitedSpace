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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(GameObject owner, Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * emitSpeed;
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody == null || collision.attachedRigidbody.gameObject != owner)
            Destroy(gameObject);
    }
}

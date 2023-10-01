using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RandomEnemy : MonoBehaviour
{
    [Header("Pathfinding")]
    public float moveSpeed = 3;
    public float maxSpeedChange = 100;
    protected Vector3 targetPosition;
    protected Vector2 desiredVelocity;
    protected NavMeshPath path;
    protected  Rigidbody2D body;
    protected Vector3[] corners;
    public bool IsWalking => desiredVelocity.magnitude > 0.1f;

    float lastTargetTime;
    public float changeTargetInterval;
    public float targetRadius = 3;
    Vector2 originalPosition;
    bool ShouldChangeTarget => Time.time >= lastTargetTime + changeTargetInterval;

    Health health;

    // Start is called before the first frame update
    protected void Start()
    {
        path = new NavMeshPath();
        corners = new Vector3[10];

        health = GetComponent<Health>();
        health.onDie.AddListener(OnDie);
        health.onHit.AddListener(OnHit);
        body = GetComponent<Rigidbody2D>();

        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    // Update is called once per frame
    protected void Update()
    {
        SelectTarget();
       // Debug.DrawLine(transform.position, targetPosition, Color.red);

        ComputeDesiredVelocity();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = body.velocity;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange * Time.fixedDeltaTime);

        body.velocity = velocity;
    }

    protected void SelectTarget()
    {
        if (!ShouldChangeTarget)
            return;

        lastTargetTime = Time.time;
        Vector2 randDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randDirection.Normalize();
        targetPosition = originalPosition + randDirection * Random.Range(0f,targetRadius);
    }

    protected void OnDie()
    {
        enabled = false;

        SFXManager.PlaySound(GlobalSFX.EnemyKilled);
    }

    protected void OnHit()
    {
        SFXManager.PlaySound(GlobalSFX.EnemyDamaged);
    }

    protected void ComputeDesiredVelocity()
    {
        if (lastTargetTime + 1 > Time.time)
            return;

        Vector2 targetDirection = targetPosition - transform.position;
        if (targetDirection.magnitude < Time.deltaTime * moveSpeed)
        {
            desiredVelocity = Vector2.zero;
            return;
        }

        targetDirection.Normalize();

        if (!HasLineOfSightToPosition(targetPosition))
        {
            UpdatePath();
            targetDirection = GetMoveAlongPath();
        }

        desiredVelocity = targetDirection * moveSpeed;
    }

    public bool HasLineOfSightToPosition(Vector3 position)
    {
        Vector2 toPos = position - transform.position;
        LayerMask layer = LayerMask.GetMask("Level");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPos.normalized, toPos.magnitude, layer);

        return hit.collider == null;
    }

    protected void UpdatePath()
    {
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
    }

    protected Vector2 GetMoveAlongPath()
    {
        path.GetCornersNonAlloc(corners);
        Vector2 toNextCorner = corners[1] - transform.position;
        toNextCorner.Normalize();

        return toNextCorner;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }
#endif
}

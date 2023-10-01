using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public float moveSpeed = 3;
    public float maxSpeedChange = 100;
    protected Vector3 targetPosition;
    protected Vector2 desiredVelocity;
    protected NavMeshPath path;
    protected  Rigidbody2D body;
    public enum TargetBehaviour { AimAtPlayer, AimAtMaxRange }
    public TargetBehaviour targetBehaviour;
    protected Vector3[] corners;

    [Header("Attack")]
    public float attackTriggerRange;
    public float attackRadius;
    public float minAttackInterval;
    protected float lastAttackTime;
    protected bool isAttacking;
    public bool IsWalking => desiredVelocity.magnitude > 0.1f;
    public float attackAnticipationDuration = 0.2f;
    public float attackDuration = 1;
    public UnityEvent onAttack = new UnityEvent();
    protected bool canHitAllies = false;

    [Header("Tracking")]
    public TrackingBehaviour trackingBehaviour;
    public enum TrackingBehaviour { Always, Range, LineOfSight}
    public float startTrackingRange = 10;
    public bool isTracking { get;private set; }

    Health health;

    // Start is called before the first frame update
    protected void Start()
    {
        path = new NavMeshPath();
        corners = new Vector3[10];

        health = GetComponent<Health>();
        health.onDie.AddListener(OnDie);
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        ProcessTracking();

        //only start moving if tracking started
        if (!isTracking)
            return;

        //Move into range
        switch (targetBehaviour)
        {
            case TargetBehaviour.AimAtPlayer:
            default:
                targetPosition = PlayerState.Instance.CenterOfMass;
                break;
            case TargetBehaviour.AimAtMaxRange:
                Vector3 fromPlayer = transform.position - PlayerState.Instance.CenterOfMass;
                targetPosition = fromPlayer.normalized * attackTriggerRange;
                break;
        }

        ComputeDesiredVelocity();

        if(Vector3.Distance(PlayerState.Instance.CenterOfMass, transform.position)<=attackTriggerRange)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = body.velocity;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange * Time.fixedDeltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange * Time.fixedDeltaTime);

        body.velocity = velocity;
    }

    protected void ProcessTracking()
    {
        if (isTracking)
            return;
        switch (trackingBehaviour)
        {
            case TrackingBehaviour.Always:
            default:
                isTracking = true;
                break;
            case TrackingBehaviour.Range:
                if (Vector2.Distance(PlayerState.Instance.CenterOfMass, transform.position)<startTrackingRange)
                    isTracking = true;
                break;
            case TrackingBehaviour.LineOfSight:
                LayerMask mask = 1 << LayerMask.NameToLayer("Level") | 1 << LayerMask.NameToLayer("Player");
                Vector2 dir = PlayerState.Instance.CenterOfMass - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 100, mask);
                if(hit.collider != null)
                {
                    if(hit.collider.CompareTag("Player"))
                        isTracking = true;
                }
                break;
        }
    }

    protected void OnDie()
    {
        enabled = false;
    }

    protected void ComputeDesiredVelocity()
    {
        if (isAttacking)
        {
            desiredVelocity = Vector2.zero;
            return;
        }

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

    protected void Attack()
    {
        if (isAttacking || Time.time < lastAttackTime+minAttackInterval)
            return;

        lastAttackTime = Time.time;
        CustomAttack();
    }

    protected virtual void CustomAttack()
    {
        StartCoroutine(AttackC());
    }

    IEnumerator AttackC()
    {
        isAttacking = true;
        onAttack.Invoke();

        yield return new WaitForSeconds(attackAnticipationDuration);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach(Collider2D hit in hits)
        {
            if(hit.attachedRigidbody == null) continue;
            if(hit.attachedRigidbody == body) continue;
            if (!canHitAllies && hit.transform.CompareTag("Enemy")) continue;
            if (hit.attachedRigidbody.TryGetComponent(out Health health))
                health.Hit(1);
        }

        yield return new WaitForSeconds(attackDuration- attackAnticipationDuration);

        isAttacking = false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackTriggerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        if(trackingBehaviour == TrackingBehaviour.Range)
            Gizmos.DrawWireSphere(transform.position, startTrackingRange);

        //Handles.Label(transform.position + Vector3.down * 0.3f, state.ToString());

        //Path
        if (path == null || path.corners == null || path.corners.Length < 2)
            return;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, path.corners[1]);

    }
#endif
}

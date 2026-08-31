using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBase : MonoBehaviour
{
    #region General Variables

    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask groundLayer;

    [Header("Patroling Stats")]
    [SerializeField] float walkPointRange = 8f;
    Vector3 walkPoint;
    bool walkPointSet;

    [Header("Attacking Stats")]
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootSpeedY;
    [SerializeField] float shootSpeedZ = 10f;
    bool alreadyAttacked;

    [Header("Attack Audio")]
    [SerializeField] AudioSource shootAudio;

    [Header("States & Detection Areas")]
    [SerializeField] float sightRange = 8f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] bool targetInSightRange;
    [SerializeField] bool targetInAttackRange;

    [Header("Stuck Detection")]
    [SerializeField] float stuckCheckTime = 2f;
    [SerializeField] float stuckThreshold = 0.1f;
    [SerializeField] float maxStuckDuration = 3f;

    float stuckTimer;
    float lastCheckTime;
    Vector3 lastPosition;

    #endregion

    private bool isDead = false;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            target = player.transform;
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        lastPosition = transform.position;
        lastCheckTime = Time.time;
    }

    private void Update()
    {
        if (isDead)
            return;

        EnemyStateUpdater();
        CheckIfStuck();
    }

    public void SetDead(bool dead)
    {
        isDead = dead;

        if (isDead)
        {
            StopAI();
        }
        else
        {
            ResetAI();
        }
    }

    private void StopAI()
    {
        CancelInvoke(nameof(ResetAttack));

        alreadyAttacked = false;
        walkPointSet = false;
        stuckTimer = 0f;

        if (agent != null && agent.enabled)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
    }

    private void ResetAI()
    {
        alreadyAttacked = false;
        walkPointSet = false;
        stuckTimer = 0f;

        lastPosition = transform.position;
        lastCheckTime = Time.time;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = false;
            agent.ResetPath();
        }
    }

    void EnemyStateUpdater()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            sightRange,
            targetLayer
        );

        targetInSightRange = hits.Length > 0;

        if (targetInSightRange && target != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                target.position
            );

            targetInAttackRange = distance <= attackRange;
        }
        else
        {
            targetInAttackRange = false;
        }

        if (!targetInSightRange && !targetInAttackRange)
        {
            Patroling();
        }
        else if (targetInSightRange && !targetInAttackRange)
        {
            ChaseTarget();
        }
        else if (targetInSightRange && targetInAttackRange)
        {
            AttackTarget();
        }
    }

    void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

        if ((transform.position - walkPoint).sqrMagnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        int attempts = 0;
        const int maxAttempts = 5;

        while (!walkPointSet && attempts < maxAttempts)
        {
            attempts++;

            Vector3 randomPoint =
                transform.position +
                new Vector3(
                    Random.Range(-walkPointRange, walkPointRange),
                    0,
                    Random.Range(-walkPointRange, walkPointRange)
                );

            if (NavMesh.SamplePosition(
                randomPoint,
                out NavMeshHit hit,
                2f,
                NavMesh.AllAreas))
            {
                walkPoint = hit.position;

                if (Physics.Raycast(
                    walkPoint,
                    -transform.up,
                    2f,
                    groundLayer))
                {
                    walkPointSet = true;
                }
            }
        }
    }

    void ChaseTarget()
    {
        agent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        agent.SetDestination(transform.position);

        Vector3 direction =
            (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    lookRotation,
                    agent.angularSpeed * Time.deltaTime
                );
        }

        if (!alreadyAttacked)
        {
            Rigidbody rb =
                Instantiate(
                    projectile,
                    shootPoint.position,
                    Quaternion.identity
                ).GetComponent<Rigidbody>();

            rb.AddForce(
                transform.forward * shootSpeedZ,
                ForceMode.Impulse
            );

            if (shootAudio != null)
            {
                shootAudio.Play();
            }

            alreadyAttacked = true;

            Invoke(
                nameof(ResetAttack),
                timeBetweenAttacks
            );
        }
    }

    void ResetAttack()
    {
        if (isDead)
            return;

        alreadyAttacked = false;
    }

    void CheckIfStuck()
    {
        if (Time.time - lastCheckTime > stuckCheckTime)
        {
            float distanceMoved =
                Vector3.Distance(
                    transform.position,
                    lastPosition
                );

            if (
                distanceMoved < stuckThreshold &&
                agent.hasPath
            )
            {
                stuckTimer += stuckCheckTime;
            }
            else
            {
                stuckTimer = 0;
            }

            if (stuckTimer >= maxStuckDuration)
            {
                walkPointSet = false;
                agent.ResetPath();
                stuckTimer = 0;
            }

            lastPosition = transform.position;
            lastCheckTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            sightRange
        );
    }
}
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBase : MonoBehaviour
{
    #region General Variables
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; //Ref al cerebro NavMesh del objeto
    [SerializeField] Transform target; //Ref a la posicion del jugador para seguirlo
    [SerializeField] LayerMask targetLayer; //Define la capa del target (Deteccion)
    [SerializeField] LayerMask groundLayer; //Define capa del suelo (puntos navegables)
    
    [Header("Patroling Stats")]
    [SerializeField] float walkPointRange = 8; //Rango maximo de margen espacial para buscar puntos navegables
    Vector3 walkPoint; //Posicion del punto a perseguir
    bool walkPointSet; //Si es falso busca punto, si no no puede buscar punto

    [Header("Atacking Stats")]
    [SerializeField] float timeBetweenAttacks = 1f; //Tiempo entre ataque y ataque
    [SerializeField] GameObject projectile; //Ref al prefab del proyectil
    [SerializeField] Transform shootPoint; //Posicion inicial del disparo
    [SerializeField] float shootSpeedY; //Potencia del disparo vertical (solo catapulta)
    [SerializeField] float shootSpeedZ = 10f; //Potencia del disparo hacia delante (siempre está)
    bool alreadyAttacked; //Se pregunta si estamos atacando para no stackear ataques

    [Header("States & Detection Areas")]
    [SerializeField] float sightRange = 8f; //Radio de la deteccion de persecucion
    [SerializeField] float attackRange = 2f; //Radio de la deteccion del ataque
    [SerializeField] bool targetInSightRange; //Determina si entra en estado perseguir
    [SerializeField] bool targetInAttackRange; //Determina si entra en estado atacar

    [Header("Stuck Detection")]
    [SerializeField] float stuckCheckTime = 2f; //Tiempo que el agente espera quieto hasta preguntarse si está stuck
    [SerializeField] float stuckThreshold = 0.1f; //Margen de deteccion del stuck
    [SerializeField] float maxStuckDuration = 3f; //Tiempo maximo de estar stuck
    float stuckTimer; //Reloj que cuenta cuanto tiempo esta stuck
    float lastCheckTime; //Define el tiempo de chequeo previo a estar stuck
    Vector3 lastPosition; //Posicion del ultimo walkpoint perseguido
    #endregion


    private void Awake()
    {
        targetInAttackRange = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;
        lastCheckTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateUpdater();
    }

    void EnemyStateUpdater()
    {
        //Accion que se encarga de la gestion de los estados de la IA
        //Esfera de deteccion fisica
        Collider[] hits = Physics.OverlapSphere(transform.position, sightRange, targetLayer);
        targetInSightRange = hits.Length > 0;
        if (targetInSightRange)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            targetInAttackRange = distance <= attackRange; 
            //Si esta persiguiendo, calcula la distancia hasta que el minimo entree en el rango de ataque
        }

        //Logica de los cambios de estado
        if (!targetInSightRange && !targetInAttackRange) Patroling(); 
        else if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        else if (targetInSightRange && targetInAttackRange) AttackTarget();
}

    void Patroling()
    {
        //Define que el objeto patrulle y genere puntos de patrulla random
        //1 - Revisa si hay un punto a patrullar
        if (!walkPointSet)
        {
            //Si no hay walkpoint, busca uno
            SearchWalkPoint();
        }
        else agent.SetDestination(walkPoint); //Si hay punto, lo persigue

        //2  -Una vez ha llegado al punto, 
        if ((transform.position - walkPoint).sqrMagnitude < 1f)
            {
                walkPointSet = false;
            }
    }
    
    void SearchWalkPoint()
    {
        //Accion que busca un punto de patrulla random si no lo hay
        int attempts = 0; //Numero de intentos de encontrar un punto nuevo
        const int maxAttempts = 5; 

        while (!walkPointSet && attempts < maxAttempts)
        {
            attempts++;
            Vector3 randomPoint = transform.position + new Vector3(Random.Range(-walkPointRange, walkPointRange), 0, Random.Range(-walkPointRange, walkPointRange));
            //
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                walkPoint =  hit.position; //Determina el Vector3 a perseguir
                if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
                {
                    walkPointSet = true; //Tenemos punto y el agarre va hacia el
                }
            }
        }
    }

    void ChaseTarget()
    {
        //Le dice al agente que persiga al target
        agent.SetDestination(target.position);
    }
    
    void AttackTarget()
    {
        //Accion que determina el ataque al objetivo

        //1 - Detener el movimiento
        agent.SetDestination(transform.position);

        //2 - Rotacion suavizada para mirar al target
        Vector3 direction = (target.position - transform.position).normalized;
        //Condicional que revisa si agente y targeyt NO se estan mirando
        if (direction!= Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, agent.angularSpeed * Time.deltaTime);
        }

        //
        //
        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        //Accion que resetea el ataque
        alreadyAttacked = false;
    }

    void CheckIfStuck()
    {
        //Accion que revisa si el agente está atrapado
        if(Time.time - lastCheckTime > stuckCheckTime)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition); 

            if (distanceMoved < stuckThreshold && agent.hasPath)
            {
                stuckTimer += stuckCheckTime;
            }
            else             {
                stuckTimer = 0f; 
            }

            if (stuckTimer >= maxStuckDuration)
            {
                walkPointSet = false;
                agent.ResetPath();
                stuckTimer = 0f;
            }

            lastPosition = transform.position;
            lastCheckTime = Time.time;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return; //Solo se ejecutan los gizmos en el editor

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack, Dead }
    [SerializeField] private EnemyState currentState = EnemyState.Idle;
    
    [Header("AI PARAMETERS")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    public float detectionRange = 10f;
    public float attackRange = 6f;
    public float patrolRange = 4f;

    [Header("ATTACK PARAMETERS")] 
    public float fireCooldown = 1.5f;
    private float fireTimer = 0f;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public Transform playerTransform;

    [Header("AREA RESTRICTION")] 
    public Transform areaCenter;
    public float areaRadius = 12f;
    
    private Vector3 target;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent =  GetComponent<NavMeshAgent>();
        PickNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EnemyState.Dead)
        {
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (Vector3.Distance(transform.position, areaCenter.position) > areaRadius)
        {
            navMeshAgent.SetDestination(areaCenter.position);
            
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
        
        StateHandler(distanceToPlayer);
    }

    public void StateHandler(float distanceToPlayer)
    {
        //Debug.Log(enemyBehaviour);
      
        switch (currentState)
        {
            case EnemyState.Idle:
                navMeshAgent.isStopped = false;
                StartCoroutine(IdleFor(1f));
                currentState = EnemyState.Patrol;
                break;
            case EnemyState.Patrol:
                PatrolRoutine();
                break;
            case EnemyState.Chase:
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(playerTransform.position);
                break;
            case EnemyState.Attack:
                navMeshAgent.isStopped = true;
                transform.LookAt(playerTransform.position);
                AttackRoutine();
                break;
            case EnemyState.Dead:
                break;
        }
    }
    
    //PATROL

    public void PatrolRoutine()
    {
        navMeshAgent.isStopped = false;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < .5f)
        {
            PickNewPatrolPoint();
        }
    }

    public void PickNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += areaCenter.position;
        
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, NavMesh.AllAreas))
        {
            target = hit.position;
            navMeshAgent.SetDestination(target);
        }
    }

    public IEnumerator IdleFor(float time)
    {
        yield return new WaitForSeconds(time);
    }
    
    //ATTACK

    public void AttackRoutine()
    {
         fireTimer -= Time.deltaTime;
         if (fireTimer <= 0)
         {
             FireProjectile();
             fireTimer = fireCooldown;
         }
    }

    public void FireProjectile()
    {
        if (projectilePrefab != null && projectileSpawn != null)
        {
            Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        }
    }
    
}

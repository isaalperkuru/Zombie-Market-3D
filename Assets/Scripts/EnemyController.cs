using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float turnSpeed = 15f;
    [SerializeField] float patrolRadius = 6f;
    [SerializeField] float patrolWaitTime = 2f;
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float searchSpeed = 3.5f;
    [Header("Attack Settings")]
    [SerializeField] int damage = 2;
    [SerializeField] float attackRate = 2f;
    [SerializeField] private float attackRange = 2f;

    private bool isSearched = false;
    private bool isAttacking = false;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    enum State
    {
        Idle,
        Search,
        Chase,
        Attack
    }

    [SerializeField] private State currentState = State.Idle;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StateCheck();

        StateExecute();
    }

    private void StateCheck()
    {
        float distanceToTarget = Vector3.Distance(player.position, transform.position);
        if(distanceToTarget <= attackRange)
        {
            currentState = State.Attack;
        }
        else if (distanceToTarget <= chaseRange && distanceToTarget > attackRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Search;
        }
    }

    private void StateExecute()
    {
        switch (currentState)
        {
            case State.Idle:
                print("Idle");
                break;
            case State.Search:
                if (!isSearched && agent.remainingDistance <= 0.1f
                    /*|| agent.hasPath && !isSearched*/)
                {
                    Vector3 agentTarget = new Vector3(agent.destination.x, transform.position.y,
                        agent.destination.z);
                    agent.enabled = false;
                    transform.position = agentTarget;
                    agent.enabled = true;

                    Invoke("Search", patrolWaitTime);
                    anim.SetBool("Walk", false);
                    isSearched = true;

                }
                print("Search");
                break;
            case State.Chase:
                print("Chase");
                Chase();
                break;
            case State.Attack:
                print("Attack");
                Attack();
                break;
        }
    }
    private void Search()
    {
        agent.isStopped = false;
        agent.speed = searchSpeed;
        isSearched = false;
        anim.SetBool("Walk", true);
        agent.SetDestination(GetRandomPosition());
    }
    private void Attack()
    {
        if(player == null)
            return;
        if (!isAttacking)
            StartCoroutine(AttackRoutine());
        anim.SetBool("Walk", false);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        LookTheTarget(player.position);
    }

    private void Chase()
    {
        if (player == null)
            return;
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        anim.SetBool("Walk", true);
        agent.SetDestination(player.position);
        
    }
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackRate);
        anim.SetTrigger("Attack");
        yield return new WaitUntil(() => IsAttackAnimationFinished("Attack"));
        isAttacking = false;
    }
    private bool IsAttackAnimationFinished(string animationName)
    {
        if(!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void LookTheTarget(Vector3 target)
    {
        Vector3 lookPos = new Vector3(target.x, transform.position.y, target.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos - transform.position), turnSpeed * Time.deltaTime);
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        return hit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        switch (currentState)
        {
            case State.Search:
                Gizmos.color = Color.blue;
                Vector3 targetPos = new Vector3(agent.destination.x, transform.position.y, agent.destination.z);
                Gizmos.DrawLine(transform.position, targetPos);
                break;
            case State.Chase:
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, player.position);
                break;
            case State.Attack:
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, player.position);
                break;
        }
    }
    public int GetDamage()
    {
        return damage;
    }
}


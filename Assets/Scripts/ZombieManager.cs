using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public ZombieAnimatorManager ZombieAnimatorManager;
    public ZombieStatManager zombieStatManager;

    public IdleState startingState; // the state this character begins with

    [Header("Flag")]
    public bool isPerformingAction;
    public bool isDead;

    [Header("Current State")]
    [SerializeField] private State currentState; // the state this character is currently on 

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurrentTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent zombieNavmeshAgent;

    [Header("Rigidbody")]
    private Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5f;

    [Header("Attack")]
    public float attackCooldownTimer;
    public float minimumAttackDistance = 1; // set this to your minimum attack distance, of the shortest range attack
    public float maximumAttackDistance = 3.5f; // set this to your maximum attack distance, of the longest range attack

    private void Awake()
    {
        currentState = startingState;
        animator = GetComponent<Animator>();
        zombieNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
        ZombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
        zombieStatManager = GetComponent<ZombieStatManager>();
    }

    private void FixedUpdate()
    {
        if (isDead == false)
        {
            HandleStateMachine();
        }
        else
        {
            zombieRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Update()
    {
        // for making sure navmesh agent is never moving out, its staying at the same position as zombie
        zombieNavmeshAgent.transform.localPosition = Vector3.zero;

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer = attackCooldownTimer - Time.deltaTime;
        }

        if (currentTarget != null)
        {
            targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurrentTarget = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void HandleStateMachine()
    {
        State nextState;

        if (currentState != null)
        {
            nextState = currentState.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }
        }


    }
}

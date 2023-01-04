using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : CharacterManager
{
    public EnemyLocomotionManager enemyLocomotionManager;
    public EnemyAnimatorHandler enemyAnimatorHandler;
    public EnemyStatsManager enemyStatsManager;
    public EnemyEffectsManager enemyEffectsManager;
    public EnemyBossManager enemyBossManager;

    public CharacterManager currentTarget;

    public Rigidbody rigidbody;
    public NavMeshAgent navMeshAgent;
    public AIState currentState;


    public bool isPerformingAction;

    [Header("A.I. Settings")]
    public float rotationSpeed = 15f;

    protected override void Awake()
    {
        base.Awake();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
        enemyAnimatorHandler = GetComponent<EnemyAnimatorHandler>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }
    protected override void FixedUpdate()
    {

    }
    private void Start()
    {
        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;
    }
    private void Update()
    {
        HandleStateMachine();
        isRotatingWithRootMotion = animator.GetBool("IsRotatingWithRootMotion");
        isInteracting = animator.GetBool("IsInteracting");

    }
    private void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            AIState nextState = currentState.Tick(this);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(AIState nextState)
    {
        currentState = nextState;
    }

}

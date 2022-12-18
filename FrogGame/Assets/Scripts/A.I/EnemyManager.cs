using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{

    public EnemyLocomotionManager enemyLocomotionManager;
    public EnemyAnimatorHandler enemyAnimatorHandler;
    public EnemyStatsManager enemyStatsManager;
    public EnemyEffectsManager enemyEffectsManager;
    public EnemyBossManager enemyBossManager;

    public CharacterManager currentTarget;

    public Rigidbody rigidbody;
    public NavMeshAgent navMeshAgent;
    public State currentState;
    

    public bool isPerformingAction;

    [Header("A.I. Settings")]
    public float detectionRadius = 20f;
    public float minimumDetectionAngle = -50f;
    public float maximimumDetectionAngle = 50f;
    public float maximimumAggressionRadius = 5f;
    public float currentRecoveryTime = 0f;
    public float rotationSpeed = 15f;

    //These only affect AI with Humanoid Stances
    [Header("Advanced AI Settings")]
    public bool allowAIToPerformBlock;
    public int blockLikelyHood = 50;
    public bool allowAIToPerformDodge;
    public int dodgeLikelyHood = 50;
    public bool allowAIToPerformParry;
    public int parryLikelyHood = 50;

    [Header("AI Archery Settings")]
    public float minimumTimeToAim = 1f;
    public float maximumTimeToAim = 3f;

    [Header("A.I Combat Settings")]
    public bool allowAIToPerformCombos;
    public bool isShiftingPhase;
    public float comboLikelyHood;
    public AICombatStyle combatStyle;

    [Header("A.I Target Information")]
    public float distanceFromTarget;
    public Vector3 targetDirection;
    public float viewableAngle;

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
    private void Start()
    {
        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;
    }
    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        isRotatingWithRootMotion =animator.GetBool("IsRotatingWithRootMotion");
        isInteracting = animator.GetBool("IsInteracting");
        canDoCombo = animator.GetBool("CanDoCombo");
        canRotate = animator.GetBool("CanRotate");
        isInvulnerable = animator.GetBool("IsInvulnerable");
        isShiftingPhase = animator.GetBool("IsShiftingPhase");
        isHoldingArrow = animator.GetBool("IsHoldingArrow");
        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsTwoHanding", isTwoHanding);
        animator.SetBool("IsBlocking", isBlocking);
        if(currentTarget!=null)
        {
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            targetDirection = currentTarget.transform.position - transform.position;
            viewableAngle = Vector3.SignedAngle(targetDirection, transform.forward,transform.up);
        }
        
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
       if(currentState!=null)
        {
            State nextState = currentState.Tick(this);

            if(nextState!=null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime > 0 )
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        if(isPerformingAction)
        {
            if(currentRecoveryTime <=0)
            {
                isPerformingAction = false;
            }
        }
    }
    

}

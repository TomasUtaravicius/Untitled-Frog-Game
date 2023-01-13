using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : CharacterManager
{
    public Rigidbody rigidbody;
    public NavMeshAgent navMeshAgent;
    public AIState currentState;
    public bool interruptedByPlayer;
    public CharacterLocomotionManager locomotionManager;
    public bool isStatic;

    public bool isPerformingAction;
    public PlayerManager playerToInteractWith;

    [Header("A.I. Settings")]
    public float rotationSpeed = 15f;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        locomotionManager = GetComponent<CharacterLocomotionManager>();
    }
    protected override void FixedUpdate()
    {
        if(!isStatic)
        {
            this.characterAnimatorHandler.CheckHeadIKWeight();
        }
        
    }
    private void Start()
    {
        navMeshAgent.enabled = false;
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
    public void InteractWithPlayer(PlayerManager player)
    {
        interruptedByPlayer = true;
        playerToInteractWith = player;
    }
    public void FinishInteractingWithPlayer()
    {
        interruptedByPlayer = false;
        playerToInteractWith = null;
    }

}

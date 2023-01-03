using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceStateHumanoid : State
{
    public AttackStateHumanoid attackState;
    public PursueStateHumanoid pursueState;
    public IdleStateHumanoid idleState;
    public ItemBasedAttackAction[] enemyAttacks;
    protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;

    [Header("State Flags")]
    bool willPerformBlock = false;
    bool willPerformDodge = false;
    bool willPerformParry = false;
 

    bool hasPerformedDodge = false;
    bool hasRandomDodgeDirection = false;
    float randomDodgeDirection;
    bool hasAmmoLoaded = false;
    float timeUntilFire;
    Quaternion targetDodgeDirection;


    private void Awake()
    {
        attackState = GetComponent<AttackStateHumanoid>();
        pursueState = GetComponent<PursueStateHumanoid>();
        idleState = GetComponent<IdleStateHumanoid>();
    }
    public override State Tick(EnemyManager enemy)
    {
        if(enemy.combatStyle == AICombatStyle.SwordAndShield)
        {
            return ProcessSwordAndShieldCombatStyle(enemy);
        }
        else if(enemy.combatStyle == AICombatStyle.Archer)
        {
            return ProcessArcherCombatStyle(enemy);
        }
        return idleState;

    }
    private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
    {
        //If the target is dead, go to idle
        if (enemy.currentTarget.isDead)
        {
            return idleState;
        }
        //If the AI is falling or performing an action, stop movement
        if (!enemy.isGrounded || enemy.isInteracting)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);
            return this;
        }

        
        
        //If AI is too far from the target, go pursue it.
        if (enemy.distanceFromTarget > enemy.maximimumAggressionRadius)
        {
            return pursueState;
        }

        //Randomize the walking pattern when in combat
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemy);
        }
        if(enemy.allowAIToPerformBlock)
        {
            RollForBlockChance(enemy);
        }
        if (enemy.allowAIToPerformDodge)
        {
            RollForDodgeChance(enemy);
        }
        if (enemy.allowAIToPerformParry)
        {
            RollForParryChance(enemy);
        }

        if (willPerformBlock)
        {
            BlockUsingOffHand(enemy);
        }
        if(willPerformDodge && enemy.currentTarget.isAttacking && enemy.distanceFromTarget<=3f)
        {
            Dodge(enemy);
        }
        if(willPerformParry)
        {

        }

        HandleRotateTowardsTarget(enemy);

        if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null && enemy.distanceFromTarget<=attackState.currentAttack.maximumDistanceNeededToAttack)
        {
            ResetStateFlags();
            return attackState;
        }

        else
        {
            GetNewAttack(enemy);
        }
        HandleMovement(enemy);
        return this;
    }
    private State ProcessArcherCombatStyle(EnemyManager enemy)
    {

        //If the target is dead, go to idle
        if (enemy.currentTarget.isDead)
        {
            return idleState;
        }
        //If the AI is falling or performing an action, stop movement
        if (!enemy.isGrounded || enemy.isInteracting)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);
            return this;
        }

        //If AI is too far from the target, go pursue it.
        if (enemy.distanceFromTarget > enemy.maximimumAggressionRadius)
        {
            ResetStateFlags();
            return pursueState;
        }

        //Randomize the walking pattern when in combat
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemy);
        }
       
        if (enemy.allowAIToPerformDodge)
        {
            RollForDodgeChance(enemy);
        }

        if (willPerformDodge && enemy.currentTarget.isAttacking && enemy.distanceFromTarget <= 3f)
        {
            Dodge(enemy);
        }

        HandleRotateTowardsTarget(enemy);

        if(!enemy.isHoldingArrow)
        {
            DrawArrow(enemy);
            AimAtTarget(enemy);
        }
        if (enemy.currentRecoveryTime <= 0 && enemy.isHoldingArrow)
        {
            ResetStateFlags();
            return attackState;
        }

        else
        {
            GetNewAttack(enemy);
        }

        if(enemy.isStationaryArcher)
        {
            enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            
        }
        else
        {
            HandleMovement(enemy);
        }
        
        return this;
    }
    protected void HandleRotateTowardsTarget(EnemyManager enemy)
    {
        //Rotate manually
        if (enemy.isPerformingAction)
        {
            Vector3 direction = enemy.currentTarget.transform.position - enemy.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = enemy.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Debug.LogWarning("Handle Rotate Towards Enemy: target rotation: " + targetRotation);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
        }
        //Rotate with navmesh
        else
        {
            //Debug.LogWarning("Rotating with navmesh");
            Vector3 relativeDirection = enemy.transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemy.rigidbody.velocity;

            enemy.navMeshAgent.enabled = true;
            enemy.navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
            enemy.rigidbody.velocity = targetVelocity;
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navMeshAgent.transform.rotation, enemy.rotationSpeed / Time.deltaTime);
        }


    }
    protected void DecideCirclingAction(EnemyManager enemy)
    {
        WalkAroundTarget(enemy);
    }
    protected virtual void WalkAroundTarget(EnemyManager enemy)
    {
        verticalMovementValue = 0.5f;
        horizontalMovementValue = Random.Range(-1, 1);

        if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
        {
            horizontalMovementValue = 0.5f;
        }
        else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
        {
            horizontalMovementValue = -0.5f;
        }
        Debug.LogWarning("Walk around Target");
    }
    protected virtual void GetNewAttack(EnemyManager enemy)
    {

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemy.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
            {
                if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }
        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemy.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
            {
                if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (attackState.currentAttack != null)
                    {
                        return;
                    }
                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        attackState.currentAttack = enemyAttackAction;
                    }
                }
            }
        }

    }

    private void RollForBlockChance(EnemyManager enemy)
    {
        int blockChance = Random.Range(0, 100);

        if (blockChance <= enemy.blockLikelyHood)
        {
            willPerformBlock = true;
        }
        else
        {
            willPerformBlock = false;
        }
    }
    private void RollForDodgeChance(EnemyManager enemy)
    {
        int dodgeChance = Random.Range(0, 100);

        if (dodgeChance <= enemy.dodgeLikelyHood)
        {
            willPerformDodge = true;
        }
        else
        {
            willPerformDodge = false;
        }
    }
    private void RollForParryChance(EnemyManager enemy)
    {
        int parryChance = Random.Range(0, 100);

        if (parryChance <= enemy.parryLikelyHood)
        {
            willPerformParry = true;
        }
        else
        {
            willPerformParry = false;
        }
    }

    private void ResetStateFlags()
    {
        randomDestinationSet = false;
        willPerformBlock = false;
        willPerformDodge = false;
        willPerformParry = false;
        hasRandomDodgeDirection = false;
        hasPerformedDodge = false;
        hasAmmoLoaded = false;
    }
    private void BlockUsingOffHand(EnemyManager enemy)
    {
        if(enemy.isBlocking == false)
        {
            if(enemy.allowAIToPerformBlock)
            {
                enemy.isBlocking = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                //enemy.characterAnimatorHandler.SetBlockingAbsorptions
            }
        }
    }
    private void Dodge(EnemyManager enemy)
    {
        if(willPerformDodge)
        {
            if(!hasRandomDodgeDirection)
            {
                
                hasRandomDodgeDirection = true;
                randomDodgeDirection = Random.Range(0, 360);
                targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
            }

            if(enemy.transform.rotation != targetDodgeDirection)
            {
                Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, targetDodgeDirection, 1f);
                enemy.transform.rotation = targetRotation;

                float targetYRotation = targetDodgeDirection.eulerAngles.y;
                float currentYRotation = enemy.transform.eulerAngles.y;
                float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                if(rotationDifference<=5)
                {
                    hasPerformedDodge = true;
                    enemy.transform.rotation = targetDodgeDirection;
                    enemy.enemyAnimatorHandler.PlayTargetAnimation("Rolling",true);
                    timeUntilFire = Random.Range(enemy.minimumTimeToAim, enemy.maximumTimeToAim);
                }
            }
        }
    }

    private void DrawArrow(EnemyManager enemy)
    {
        if(!enemy.isTwoHanding)
        {
            enemy.isTwoHanding = true;
            enemy.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
        }
        else
        {
            hasAmmoLoaded = true;
            enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
            enemy.characterInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(enemy);
        }
    }

    private void AimAtTarget(EnemyManager enemy)
    {
        timeUntilFire = Random.Range(enemy.minimumTimeToAim, enemy.maximumTimeToAim);
        enemy.currentRecoveryTime = timeUntilFire;
    }
    private void HandleMovement(EnemyManager enemy)
    {
        if(enemy.distanceFromTarget<=enemy.stoppingDistance)
        {
            enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        }
        else
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        }
    }
}

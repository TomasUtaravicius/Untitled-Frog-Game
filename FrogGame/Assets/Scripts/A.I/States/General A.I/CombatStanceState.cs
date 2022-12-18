using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueState pursueState;
    public IdleState idleState;
    public EnemyAttackAction[] enemyAttacks;
    protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;
    public override State Tick(EnemyManager enemy)
    {
        if(enemy.currentTarget.isDead)
        {
            return idleState;
        }
        
        enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
        enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        attackState.hasPerformedAttack = false;
        if (enemy.isInteracting)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);
            return this;
        }
        if (enemy.distanceFromTarget> enemy.maximimumAggressionRadius)
        {
            return pursueState;
        }
        if(!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemy);
        }

        HandleRotateTowardsTarget(enemy);

        if(enemy.currentRecoveryTime<=0 && attackState.currentAttack!=null)
        {
            randomDestinationSet = false;
            return attackState;
        }
        
        else
        {
            GetNewAttack(enemy);
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
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
        }
        //Rotate with navmesh
        else
        {
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

        if(horizontalMovementValue<=1 && horizontalMovementValue>=0)
        {
            horizontalMovementValue = 0.5f;
        }
        else if(horizontalMovementValue>=-1 && horizontalMovementValue <0)
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
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

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
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

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
}

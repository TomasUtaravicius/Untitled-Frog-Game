using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;
    public PursueState pursueTargetState;
    public RotateTowardsTargetState rotateTowardsTargetState;
    public EnemyAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;
    public override State Tick(EnemyManager enemy)
    {
        float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
        RotateTowardsTargetWhileAttacking(enemy);
        if(distanceFromTarget> enemy.maximimumAggressionRadius)
        {
            return pursueTargetState;
        }
        if (willDoComboOnNextAttack && enemy.canDoCombo)
        {
            if (enemy.canDoCombo)
                AttackTargetWithCombo(enemy);
            else
            {
                hasPerformedAttack = false;
                willDoComboOnNextAttack = false;
            }
                
        }
        if (!hasPerformedAttack)
        {
            AttackTarget(enemy);
            RollForComboChance(enemy);
        }
        if(willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this;
        }
        return rotateTowardsTargetState;
    }
    private void AttackTarget(EnemyManager enemy)
    {
        enemy.isUsingLeftHand = !currentAttack.rightHandedAction;
        enemy.isUsingRightHand = currentAttack.rightHandedAction;
        enemy.enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemy.enemyAnimatorHandler.PlayWeaponTrailFX();
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy)
    {
        enemy.isUsingLeftHand = !currentAttack.rightHandedAction;
        enemy.isUsingRightHand = currentAttack.rightHandedAction;
        willDoComboOnNextAttack = false;
        enemy.enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemy.enemyAnimatorHandler.PlayWeaponTrailFX();
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }
    
    private void RotateTowardsTargetWhileAttacking(EnemyManager enemy)
    {
        //Rotate manually
        if (enemy.canRotate && enemy.isInteracting)
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
    }

    private void RollForComboChance(EnemyManager enemy)
    {
        float comboChance = Random.Range(0, 100);

        if(enemy.allowAIToPerformCombos && comboChance <= enemy.comboLikelyHood)
        {
            if(currentAttack.comboAction!=null)
            {
                willDoComboOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }
           
        }
    }
}

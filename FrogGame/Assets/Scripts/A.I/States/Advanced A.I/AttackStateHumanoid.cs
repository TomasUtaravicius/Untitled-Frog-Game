using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateHumanoid : State
{
    public CombatStanceStateHumanoid combatStanceState;
    public PursueStateHumanoid pursueTargetState;
    public RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
    public ItemBasedAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    private void Awake()
    {
        combatStanceState = GetComponent<CombatStanceStateHumanoid>();
        pursueTargetState = GetComponent<PursueStateHumanoid>();
        rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
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
        else
        {
            return this;
        }
    }
    private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
    {
        RotateTowardsTargetWhileAttacking(enemy);
        if (enemy.distanceFromTarget > enemy.maximimumAggressionRadius)
        {
            return pursueTargetState;
        }
        if (willDoComboOnNextAttack)
        {
            if (enemy.canDoCombo)
                AttackTargetWithCombo(enemy);
        }
        if (!hasPerformedAttack)
        {
            AttackTarget(enemy);
            RollForComboChance(enemy);
        }
        /*if (willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this;
        }*/

        ResetStateFlags();
        return rotateTowardsTargetState;
    }
    private State ProcessArcherCombatStyle(EnemyManager enemy)
    {

        RotateTowardsTargetWhileAttacking(enemy);
        if (enemy.isInteracting)
        {
            return this;
        }
        if(enemy.currentTarget.isDead)
        {
            ResetStateFlags();
            enemy.currentTarget = null;
            return this;
        }
        if (enemy.distanceFromTarget > enemy.maximimumAggressionRadius)
        {
            ResetStateFlags();
            return pursueTargetState;
        }
        if (!hasPerformedAttack)
        {
            FireAmmo(enemy);
            
        }
        ResetStateFlags();
        return rotateTowardsTargetState;
    }
    private void AttackTarget(EnemyManager enemy)
    {
        currentAttack.PerformAttackAction(enemy);
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy)
    {
        currentAttack.PerformAttackAction(enemy);
        willDoComboOnNextAttack = false;
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = false;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhileAttacking(EnemyManager enemy)
    {
        //Rotate manually
        if (enemy.canRotate && enemy.isInteracting)
        {
            Debug.LogWarning("ROTATING TOWARDS PLAYER DURING ATTACK");
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

        if (enemy.allowAIToPerformCombos && comboChance <= enemy.comboLikelyHood)
        {
            if (currentAttack.actionCanCombo)
            {
                willDoComboOnNextAttack = true;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }

        }
    }
    private void ResetStateFlags()
    {
        willDoComboOnNextAttack = false;
        hasPerformedAttack = false;
    }

    private void FireAmmo(EnemyManager enemy)
    {
        if(enemy.isHoldingArrow)
        {
            hasPerformedAttack = true;
            enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
            enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
        }
    }
}

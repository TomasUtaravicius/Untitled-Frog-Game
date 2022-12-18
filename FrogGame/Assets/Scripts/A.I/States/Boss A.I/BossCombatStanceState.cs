using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatStanceState : CombatStanceState
{
    public bool hasPhaseShifted;
    [Header("Second Phase Attacks")]
    public EnemyAttackAction[] secondPhaseAttacks;
    protected override void GetNewAttack(EnemyManager enemyManager)
    {
        if(hasPhaseShifted)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);


            int maxScore = 0;

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
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
        else
        {
            base.GetNewAttack(enemyManager);
        }
        
    }
    protected override void WalkAroundTarget(EnemyManager enemy)
    {
        if(hasPhaseShifted)
        {
            verticalMovementValue = 1f;
            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 1f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -1f;
            }
            Debug.LogWarning("Walk around Target");
        }
        else
        {
            base.WalkAroundTarget(enemy);
        }       
        
    }
}

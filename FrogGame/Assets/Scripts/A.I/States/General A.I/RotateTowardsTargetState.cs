using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetState : State
{
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemy)
    {
        enemy.animator.SetFloat("Vertical", 0);
        enemy.animator.SetFloat("Horizontal", 0);

        if(enemy.isInteracting)
        {
            return this; //hen we enter the state we will still be interacting from the attack animation so we pause here until it has finished.
        }
        if(enemy.viewableAngle >=100 && enemy.viewableAngle <=180 && !enemy.isInteracting)
        {
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Behind Turn", true);
            return combatStanceState;
        }
        else if(enemy.viewableAngle <=-101 && enemy.viewableAngle >=-180 && !enemy.isInteracting)
        {
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Behind Turn", true);
            return combatStanceState;
        }
        else if(enemy.viewableAngle <= -45 && enemy.viewableAngle >= -100 && !enemy.isInteracting)
        {
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Right Turn", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle <= 45 && enemy.viewableAngle >= 100 && !enemy.isInteracting)
        {
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Left Turn", true);
            return combatStanceState;
        }
        return combatStanceState;
    }
}

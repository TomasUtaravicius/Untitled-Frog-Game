using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetStateHumanoid : State
{
    public CombatStanceStateHumanoid combatStanceState;
    private void Awake()
    {
        combatStanceState = GetComponent<CombatStanceStateHumanoid>();
    }
    public override State Tick(EnemyManager enemy)
    {
        enemy.animator.SetFloat("Vertical", 0);
        enemy.animator.SetFloat("Horizontal", 0);

        if (enemy.isInteracting)
        {
            return this; //hen we enter the state we will still be interacting from the attack animation so we pause here until it has finished.
        }
        Debug.Log("ANGLE TO PLAYER: " + enemy.viewableAngle);
        if (enemy.viewableAngle >= 101 && enemy.viewableAngle <= 180 && !enemy.isInteracting)
        {
            Debug.Log("ROTATING LEFT 180 DEGREES");
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn Back Left", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle <= -101 && enemy.viewableAngle >= -180 && !enemy.isInteracting)
        {
            Debug.Log("ROTATING RIGHT 180 DEGREES");
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn Back Right", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle <= -45 && enemy.viewableAngle >= -100 && !enemy.isInteracting)
        {
            Debug.Log("ROTATING RIGHT 90 Degrees");
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn Right", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle >= 45 && enemy.viewableAngle <= 100 && !enemy.isInteracting)
        {
            Debug.Log("ROTATING LEFT 90 Degrees");
            enemy.enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn Left", true);
            return combatStanceState;
        }
        return combatStanceState;
    }
}

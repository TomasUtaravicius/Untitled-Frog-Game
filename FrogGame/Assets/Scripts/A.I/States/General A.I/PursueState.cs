using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : State
{
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager aiCharacter)
    {

        HandleRotateTowardsTarget(aiCharacter);

        if (aiCharacter.isInteracting)
        {
            return this;
        }
        if (aiCharacter.isPerformingAction)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        }

        if (aiCharacter.distanceFromTarget > aiCharacter.maximimumAggressionRadius)
        {
            aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }
        
        if(aiCharacter.distanceFromTarget<= aiCharacter.maximimumAggressionRadius)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
        
    }
    private void HandleRotateTowardsTarget(EnemyManager aiCharacter)
    {
        //Rotate manually
        if (aiCharacter.isPerformingAction)
        {
            Vector3 direction = aiCharacter.currentTarget.transform.position - aiCharacter.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = aiCharacter.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
        }
        //Rotate with navmesh
        else
        {
            Vector3 relativeDirection = aiCharacter.transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = aiCharacter.rigidbody.velocity;

            aiCharacter.navMeshAgent.enabled = true;
            aiCharacter.navMeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
            aiCharacter.rigidbody.velocity = targetVelocity;
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
        }


    }
}

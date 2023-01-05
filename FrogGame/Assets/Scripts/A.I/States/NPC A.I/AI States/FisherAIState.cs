using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisherAIState : AIState
{
    [SerializeField]
    FishingAIAction fishingAction;
    [SerializeField]
    float stateDuration;
    float stateTimeElapsed;

    [SerializeField]
    Transform fishingSpot;

    public override AIState Tick(AIManager aiCharacter)
    {
        aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
        if (aiCharacter.isInteracting)
        {
            return this;
        }
        if(RotateTowardsTarget(aiCharacter))
        {
            fishingAction.PerformAction(aiCharacter);
        }
        return this;
    }
    private bool RotateTowardsTarget(AIManager aiCharacter)
    {
        //Rotate manually
  
            Vector3 direction = fishingSpot.transform.position - aiCharacter.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = aiCharacter.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);

            if(aiCharacter.transform.rotation==targetRotation)
            {
            return true;
            }
            else
        {
            return false;
        }
        
    }


}

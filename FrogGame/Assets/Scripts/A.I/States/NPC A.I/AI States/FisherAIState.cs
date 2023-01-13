using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisherAIState : AIState
{
    [SerializeField]
    FishingAIAction fishingAction;

   // [SerializeField]
   // SaySomethingAIAction interactWithPlayerAction;
    [SerializeField]
    SaySomethingAIState saySomethingState;
    [SerializeField]
    float stateDuration;
    float stateTimeElapsed;

    [SerializeField]
    Transform fishingSpot;

    public override AIState Tick(AIManager aiCharacter)
    {
        aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);

        if(aiCharacter.interruptedByPlayer)
        {
            Debug.Log("Interrupted by the player");
            fishingAction.StopPerformingAction(aiCharacter);
            saySomethingState.StartState();
            saySomethingState.interruptedState = this;
            return saySomethingState;
            
        }
        if (aiCharacter.isInteracting)
        {
            return this;
        }
        if(RotateTowardsTarget(aiCharacter))
        {
            aiCharacter.characterAnimatorHandler.EraseHeadIK();
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
            //aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = 0f;
            characterInputs.CameraRotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            // Apply inputs to character
            aiCharacter.locomotionManager.SetInputs(ref characterInputs);

        if (aiCharacter.transform.rotation==targetRotation)
            {
            return true;
            }
            else
        {
            return false;
        }
        
    }


}

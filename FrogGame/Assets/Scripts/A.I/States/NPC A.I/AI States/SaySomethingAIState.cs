using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaySomethingAIState : AIState
{
    public AIState interruptedState;
    public SaySomethingAIAction saySomethingAction;
    public string animation;
    public AudioClip saySomethingAudio;
    public float stateDuration;
    float stateTimeElapsed;
    float begunTime;
    bool isSayingSomething = false;
    public void StartState()
    {
        begunTime = Time.time;

    }
    public override AIState Tick(AIManager aiCharacter)
    {
        stateTimeElapsed = Time.time - begunTime;

        if (stateTimeElapsed > stateDuration)
        {
            isSayingSomething = false;
            aiCharacter.interruptedByPlayer = false;
            return interruptedState;
        }
        if (aiCharacter.isInteracting)
            return this;

        if (isSayingSomething)
            return this;
        if (RotateTowardsTarget(aiCharacter))
        {
            isSayingSomething = true;
            saySomethingAction.PerformAction(aiCharacter);
            return this;
        }
       


        return this;

    }

    private bool RotateTowardsTarget(AIManager aiCharacter)
    {
        //Rotate manually

        Vector3 direction = aiCharacter.playerToInteractWith.transform.position - aiCharacter.transform.position;
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
        if (Vector3.SignedAngle(aiCharacter.transform.position, aiCharacter.playerToInteractWith.transform.position, Vector3.up)<0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}

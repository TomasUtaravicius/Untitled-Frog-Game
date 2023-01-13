using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIState : AIState
{
    IdleDialogueAIState idleDialogueState;

    private void Awake()
    {
        idleDialogueState = GetComponent<IdleDialogueAIState>();
    }
    public override AIState Tick(AIManager aiCharacter)
    {
        
        if (aiCharacter.interruptedByPlayer)
        {
            idleDialogueState.StartState();
            idleDialogueState.interruptedState = this;
            return idleDialogueState;

        }
        if (aiCharacter.isInteracting)
        {
            return this;
        }
  
        return this;
    }


}

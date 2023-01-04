using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisherAIState : AIState
{
    [SerializeField]
    FishingAIAction fishingAction;

    public override AIState Tick(AIManager character)
    {
        if(character.isInteracting)
        {
            return this;
        }

        fishingAction.PerformAction(character);
        return this;
    }

  
}

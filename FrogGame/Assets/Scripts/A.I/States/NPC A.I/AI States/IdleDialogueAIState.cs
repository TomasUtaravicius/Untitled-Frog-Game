using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleDialogueAIState : AIState
{
    public AIState interruptedState;
    public List<SaySomethingAIAction> talkingActions;
    public SaySomethingAIAction currentTalkingAction;
    public float stateDuration;
    float stateTimeElapsed;
    float begunTime;
    bool isSayingSomething = false;
    public void StartState()
    {
        begunTime = Time.time;
        PickVoiceLine();

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

        currentTalkingAction.PerformAction(aiCharacter);
        isSayingSomething = true;
        return this;

    }
    private void PickVoiceLine()
    {
        //currentTalkingAction = talkingActions[]
        SaySomethingAIAction nextTalkingAction = talkingActions[Random.Range(0, talkingActions.Count)];
        //SaySomethingAIAction currentTalkingAction 
        while (nextTalkingAction==currentTalkingAction)
        {
            nextTalkingAction = talkingActions[Random.Range(0, talkingActions.Count)];
        }
        currentTalkingAction = nextTalkingAction;
        
        stateDuration = currentTalkingAction.actionDuration;
        
    }
}

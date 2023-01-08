using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Actions/Say Something Action")]
public class SaySomethingAIAction : AIAction
{
    [SerializeField]
    AudioClip actionDialogueAudio;
    [SerializeField]
    string dialoguePiece;
    [SerializeField]
    float actionDuration;
    [SerializeField]
    string anim;
    [SerializeField]
    bool fullAttention;
    [SerializeField]
    bool overrideAnimation;
    public override void PerformAction(AIManager aiCharacter)
    {
        if(overrideAnimation)
        {
            aiCharacter.characterAnimatorHandler.PlayTargetAnimation(anim, true);
            aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
        }
        aiCharacter.characterSoundFXManager.SayDialogue(actionDialogueAudio);
        GameDialogueManager.instance.EnableDialogue(actionDuration, dialoguePiece);

        
    }

    
}

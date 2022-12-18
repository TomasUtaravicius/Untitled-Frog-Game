using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isInvulnerable = "IsInvulnerable";
    public bool isInvulnerableStatus = false;
    public string isInteractingBool = "IsInteracting";
    public bool isInteractingStatus = false;
    public string isFiringSpellBool = "IsFiringSpell";
    public bool isFiringSpellStatus = false;
   
    public bool isMirroredStatus = false;
    public string isMirrored = "IsMirrored";

    public string canRotateBool = "CanRotate";
    public bool canRotateStatus = true;
    public string isRotatingWithRootMotion = "IsRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterManager character = animator.GetComponent<CharacterManager>();
        character.isUsingLeftHand = false;
        character.isUsingRightHand = false;
        character.isAttacking = false;

        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isInvulnerable, isInvulnerableStatus);
        animator.SetBool(isMirrored, isMirroredStatus);

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBoolAI : ResetAnimatorBool
{
    public string isShiftingPhase = "IsShiftingPhase";
    public bool isShiftingStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(isShiftingPhase, isShiftingStatus);
    }
}

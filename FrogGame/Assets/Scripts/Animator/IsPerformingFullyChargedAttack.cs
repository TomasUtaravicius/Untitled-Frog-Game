using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPerformingFullyChargedAttack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsPerformingFullyCharged", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class CharacterAnimatorHandler : MonoBehaviour
{
    protected CharacterManager character;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    bool handIKWeightsReset = false;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }
    public virtual void CanRotate()
    {
        character.animator.SetBool("CanRotate", true);
    }
    public virtual void StopRotation()
    {
        character.animator.SetBool("CanRotate", false);
    }

    public virtual void EnableCombo()
    {
        character.animator.SetBool("CanDoCombo", true);
    }
    public virtual void DisableCombo()
    {
        character.animator.SetBool("CanDoCombo", false);
    }
    public  virtual void EnableIsInvulnerable()
    {
        character.animator.SetBool("IsInvulnerable", true);
    }
    public virtual void DisableIsInvulnerable()
    {
        character.animator.SetBool("IsInvulnerable", false);
    }
    public virtual void EnableCanBeRiposted()
    {
        character.canBeRiposted = true;
        character.canDoCombo = false;
    }
    public virtual void DisableCanBeRiposted()
    {
        character.canBeRiposted = false;
    }
    public virtual void EnableIsParrying()
    {
        character.isParrying = true;
    }
    public virtual void DisableIsParrying()
    {
        character.isParrying = false;
    }
    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
    {
        character.animator.SetBool("CanRotate", canRotate);
        character.animator.applyRootMotion = isInteracting;
        character.animator.SetBool("IsInteracting", isInteracting);
        character.animator.SetBool("IsMirrored", mirrorAnim);
        character.animator.CrossFade(targetAnim, 0.2f);
    }
    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        character.animator.SetBool("IsRotatingWithRootMotion", true);
        character.animator.applyRootMotion = isInteracting;
        character.animator.SetBool("IsInteracting", isInteracting);
        character.animator.CrossFade(targetAnim, 0.2f);
    }
    public virtual void TakeCriticalDamageAnimationEvent()
    {
        character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage);
        character.pendingCriticalDamage = 0;
    }
    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHanding)
    {
        if(isTwoHanding)
        {
            if(rightHandIKTarget!= null)
            {
                rightHandConstraint.data.target = rightHandIKTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if(leftHandIKTarget!=null)
            {
                leftHandConstraint.data.target = leftHandIKTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
        else
        {
            if(rightHandIKTarget!=null)
            {
                rightHandConstraint.data.target = null;
            }
            if(leftHandIKTarget!=null)
            {
                leftHandConstraint.data.target = null;
            }
            
            
        }
        rigBuilder.Build();
    }
    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHand)
    {
        if(character.isInteracting)
        {
            return;
        }

        if(handIKWeightsReset)
        {
            handIKWeightsReset = false;
            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.target = rightHandIK.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.target = leftHandIK.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
    }
    public virtual void EraseHandIKForWeapon()
    {
        handIKWeightsReset = true;
        if(rightHandConstraint.data.target!=null)
        {
            rightHandConstraint.data.targetPositionWeight = 0;
            rightHandConstraint.data.targetRotationWeight = 0;
        }

        if(leftHandConstraint.data.target!=null)
        {
            leftHandConstraint.data.targetPositionWeight = 0;
            leftHandConstraint.data.targetRotationWeight = 0;
        }
        

        
    }



}

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
    public MultiAimConstraint headLookContraint;
    public MultiAimConstraint chestLookContraint;

    bool handIKWeightsReset = false;
    bool headLookWeightReset = false;
    public HeadLookIKTarget lookTarget;
    int vertical;
    int horizontal;
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
    public void PlayTargetAnimation(string targetAnim, bool isInteracting, float timer, bool canRotate = false, bool mirrorAnim = false)
    {
        character.animator.SetBool("CanRotate", canRotate);
        character.animator.applyRootMotion = isInteracting;
        character.animator.SetBool("IsInteracting", isInteracting);
        character.animator.SetBool("IsMirrored", mirrorAnim);
        character.animator.CrossFade(targetAnim, timer);
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
    public virtual void SetHeadIKToTarget(HeadLookIKTarget target,float weight)
    {
        if(target!=null)
        {
            headLookWeightReset = true;
            lookTarget.transform.position = target.transform.position;

            headLookContraint.weight = weight;
            headLookContraint.weight = weight;
            chestLookContraint.weight = weight;
            chestLookContraint.weight = weight;
        }
                
            
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
    public virtual void CheckHeadIKWeight()
    {
        /*if(headLookWeightReset)
        {
            headLookWeightReset = false;
            headLookContraint.weight = 1;
            headLookContraint.weight = 1;
            chestLookContraint.weight = 1;
            chestLookContraint.weight = 1;
        }*/
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
    public virtual void EraseHeadIK()
    {
        headLookWeightReset = true;
        headLookContraint.weight = 0;
        headLookContraint.weight = 0;
        chestLookContraint.weight = 0;
        chestLookContraint.weight = 0;
    }
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1f;
        }
        else
        {
            v = 0f;
        }
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1f;
        }
        else
        {
            h = 0f;
        }
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }
        character.animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
    }


    private void OnAnimatorMove()
    {
        if (!character.isInteracting)
        {
            return;
        }

        float delta = Time.fixedDeltaTime;
        Vector3 deltaPosition = character.animator.deltaPosition;
        Vector3 velocity = deltaPosition / delta;
        //character..AddVelocity(velocity);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    public CharacterManager character;
    [Header("Character FX")]
    public GameObject bloodSplatterFX;
    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    [Header("Current Range FX")]
    public GameObject instantiatedFXModel;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
   public virtual void PlayWeaponFX(bool isLeft)
    {
        if(!isLeft)
        {
            if(rightWeaponFX!=null)
            {
                rightWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if(leftWeaponFX!=null)
            {
                leftWeaponFX.PlayWeaponFX();
            }

        }
    }
    public virtual void StopWeaponFX(bool isLeft)
    {
        if (!isLeft)
        {
            if (rightWeaponFX != null)
            {
                rightWeaponFX.StopWeaponFX();
            }
        }
        else
        {
            if (leftWeaponFX != null)
            {
                leftWeaponFX.StopWeaponFX();
            }

        }
    }
    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
    public virtual void InteruptEffect()
    {
        if(instantiatedFXModel!=null)
        {
            Destroy(instantiatedFXModel);
        }
        if(character.isHoldingArrow)
        {

            character.animator.SetBool("IsHoldingArrow",false);
            Animator rangedWeaponAnimator = character.characterWeaponSlotManager.rightHandSlot.currentItemModel.GetComponentInChildren<Animator>();

            if(rangedWeaponAnimator!=null)
            {
                rangedWeaponAnimator.SetBool("IsDrawn", false);
                rangedWeaponAnimator.Play("Bow_TH_Fire_01");
            }

        }
        if(character.isAiming)
        {
            character.animator.SetBool("IsAiming", false);
        }
    }
}

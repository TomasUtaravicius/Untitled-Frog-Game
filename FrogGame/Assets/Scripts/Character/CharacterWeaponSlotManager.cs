using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    protected CharacterManager character;


    [Header("Unarmed Weapon")]
    public WeaponItem unarmedWeapon;

    [Header("Weapon Slots")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    [Header("Hand IK Targets")]
    public RightHandIKTarget rightHandIKTarget;
    public LeftHandIKTarget leftHandIKTarget;

    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        LoadWeaponHolderSlots();
    }
    public virtual void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }
    public virtual void LoadBothWeaponsOnSlots()
    {
        if (character.characterInventoryManager.rightWeapon != null)
        {
            Debug.Log("Load Right Weapon on Slot: " + gameObject.name);
            LoadWeaponOnSlot(character.characterInventoryManager.rightWeapon, false);
        }
        if (character.characterInventoryManager.leftWeapon != null)
        {
            LoadWeaponOnSlot(character.characterInventoryManager.leftWeapon, true);
        }
    }
    public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                //character.characterAnimatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if (character.isTwoHanding)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    character.characterAnimatorHandler.PlayTargetAnimation("Left Arm Empty", false, true);

                    if(weaponItem.weaponType==WeaponType.Sword)
                    character.characterAnimatorHandler.PlayTargetAnimation("Two_Hand_Idle_01", false, true);
                }
                else
                {
                    character.characterAnimatorHandler.PlayTargetAnimation("Both Arms Empty", false, true);
                    if(backSlot!=null)
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();

                if(weaponItem.weaponAnimator!=null)
                {
                    character.animator.runtimeAnimatorController = weaponItem.weaponAnimator;
                }
                
                LoadTwoHandIKTargets(character.isTwoHanding);
            }
        }
        else
        {
            weaponItem = unarmedWeapon;
            if (isLeft)
            {
                character.characterInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(unarmedWeapon);
                LoadLeftWeaponDamageCollider();
                //character.characterAnimatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                character.characterInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(unarmedWeapon);
                LoadRightWeaponDamageCollider();
                character.animator.runtimeAnimatorController = weaponItem.weaponAnimator;
            }


        }

    }
    protected virtual void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        leftHandDamageCollider.currentWeaponDamage = character.characterInventoryManager.leftWeapon.baseDamage;
        leftHandDamageCollider.characterManager = character;
        leftHandDamageCollider.poiseBreak = character.characterInventoryManager.leftWeapon.poiseBreak;
        leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;
        character.characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }
    
    protected virtual void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamageCollider.currentWeaponDamage = character.characterInventoryManager.rightWeapon.baseDamage;
        rightHandDamageCollider.characterManager = character;
        rightHandDamageCollider.poiseBreak = character.characterInventoryManager.rightWeapon.poiseBreak;
        rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;
        character.characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    protected virtual void LoadTwoHandIKTargets(bool isTwoHanding)
    {
        leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

        Debug.LogWarning("Load Two Hand IK Targets");
        character.characterAnimatorHandler.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHanding);
    }

    public virtual void OpenDamageCollider()
    {
        if (character.isUsingRightHand)
        {
            rightHandDamageCollider.EnableDamageCollider();
            character.characterEffectsManager.PlayWeaponFX(character.isUsingLeftHand);
            character.characterSoundFXManager.PlayRandomWeaponWhoosh();
        }
        else if (character.isUsingLeftHand)
        {
            leftHandDamageCollider.EnableDamageCollider();
            character.characterEffectsManager.PlayWeaponFX(character.isUsingLeftHand);
            character.characterSoundFXManager.PlayRandomWeaponWhoosh();
        }

    }

    public virtual void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
            character.characterEffectsManager.StopWeaponFX(character.isUsingLeftHand);
        }
        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
            character.characterEffectsManager.StopWeaponFX(character.isUsingLeftHand);
        }
    }
    public virtual void GrantWeaponAttackingPoiseBonus()
    {
        WeaponItem currentWeaponBeingUsed = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
        character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + currentWeaponBeingUsed.offensivePoiseBonus;
    }
    public virtual void ResetWeaponAttackingPoiseBonus()
    {
        character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armorPoiseBonus;
    }
}

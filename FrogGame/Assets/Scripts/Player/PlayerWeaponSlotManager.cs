using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    PlayerManager player;
    public WeaponHolderSlot sideSlot;




    [Header("Idle Hand IK Targets")]
    public RightHandIKTarget idleRightHandIKTarget;
    public LeftHandIKTarget idleLeftHandIKTarget;
    protected override void Awake()
    {
        player = GetComponent<PlayerManager>();
        LoadWeaponHolderSlots();
        LoadBothWeaponsOnSlots();
    }

    public override void LoadWeaponHolderSlots()
    {
        Debug.LogWarning("Load Weapon Holder Slots");
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
            else if (weaponSlot.isSideSlot)
            {
                sideSlot = weaponSlot;
            }
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }
    public override void LoadBothWeaponsOnSlots()
    {

        Debug.Log("Load Right Weapon on Slot: " + gameObject.name);
        LoadWeaponOnSlot(player.playerInventoryManager.meleeWeapon, false);


        /* Debug.LogWarning("Load Both Weapons On Slots");
         if (character.characterInventoryManager.rightWeapon != null)
         {
             Debug.Log("Load Right Weapon on Slot: " + gameObject.name);
             LoadWeaponOnSlot(character.characterInventoryManager.rightWeapon, false);
         }
         if (character.characterInventoryManager.leftWeapon != null)
         {
             LoadWeaponOnSlot(character.characterInventoryManager.leftWeapon, true);
         }*/
    }
    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isMelee)
    {

        if (player.isInCombat || player.isDrawing)
        {
            if (player.inputHandler.twoHandFlag)
            {
                if (weaponItem.weaponType == WeaponType.Bow)
                {
                    player.playerAnimatorHandler.PlayTargetAnimation("Both Arms Empty", false, true);
                }
                else
                {
                    player.playerAnimatorHandler.PlayTargetAnimation("Two_Hand_Idle_01", false, true);
                }

            }
            else
            {
                player.playerAnimatorHandler.PlayTargetAnimation("Both Arms Empty", false, true);
                backSlot.UnloadWeaponAndDestroy();
            }

            Debug.Log("Player is in Combat, loading in right hand slot");
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            backSlot.UnloadWeaponAndDestroy();
            LoadTwoHandIKTargets(player.inputHandler.twoHandFlag);
            LoadDamageCollider();
            player.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            player.animator.runtimeAnimatorController = weaponItem.weaponAnimator;
        }
        else
        {
            Debug.Log("Player is not in combat, loading in backslot");
            backSlot.LoadWeaponModel(player.playerInventoryManager.meleeWeapon);
            backSlot.currentWeapon = player.playerInventoryManager.meleeWeapon;
            rightHandSlot.UnloadWeaponAndDestroy();
            LoadTwoHandIKTargets(false);
            player.playerAnimatorHandler.PlayTargetAnimation("Both Arms Empty", false, true);

        }
    
        /*if(weaponItem!=null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                player.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                //player.playerAnimatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if (player.inputHandler.twoHandFlag )
                {
                    if(weaponItem.weaponType==WeaponType.Bow)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        player.playerAnimatorHandler.PlayTargetAnimation("Both Arms Empty", false, true);
                    }
                    else
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        player.playerAnimatorHandler.PlayTargetAnimation("Two_Hand_Idle_01", false, true);
                    }
                    
                }
                else
                {
                    player.playerAnimatorHandler.PlayTargetAnimation("Both Arms Empty", false, true);
                    backSlot.UnloadWeaponAndDestroy();
                }

                
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadTwoHandIKTargets(player.inputHandler.twoHandFlag);
                LoadRightWeaponDamageCollider();
                player.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                player.animator.runtimeAnimatorController = weaponItem.weaponAnimator;
            }
        }
        else
        {
            if(isLeft)
            {
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(unarmedWeapon);
                LoadLeftWeaponDamageCollider();
                player.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, unarmedWeapon);
                player.playerInventoryManager.leftWeapon = unarmedWeapon;
                //player.playerAnimatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(unarmedWeapon);
                LoadRightWeaponDamageCollider();
                player.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, unarmedWeapon);
                player.playerInventoryManager.rightWeapon = unarmedWeapon;
                player.animator.runtimeAnimatorController = weaponItem.weaponAnimator;
            }
            
            
        }*/
        
    }

    public void LoadDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamageCollider.currentWeaponDamage = player.playerInventoryManager.meleeWeapon.baseDamage;
        rightHandDamageCollider.characterManager = player;
        rightHandDamageCollider.poiseBreak = player.playerInventoryManager.meleeWeapon.poiseBreak;
        rightHandDamageCollider.teamIDNumber = player.characterStatsManager.teamIDNumber;
        player.characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }


    protected override void LoadTwoHandIKTargets(bool isTwoHanding)
    {
        if(player.isArmed)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        }
        else
        {
            leftHandIKTarget = idleLeftHandIKTarget;
            rightHandIKTarget = idleRightHandIKTarget;
        }
        

        Debug.LogWarning("Load Two Hand IK Targets");
        player.characterAnimatorHandler.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHanding);
    }

    public override void OpenDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
        player.characterEffectsManager.PlayWeaponFX(false);
        player.characterSoundFXManager.PlayRandomWeaponWhoosh();
    }

    public override void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
        player.characterEffectsManager.StopWeaponFX(false);
    }
    public override void GrantWeaponAttackingPoiseBonus()
    {
        WeaponItem currentWeaponBeingUsed = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;
        player.characterStatsManager.totalPoiseDefense = player.characterStatsManager.totalPoiseDefense + currentWeaponBeingUsed.offensivePoiseBonus;
    }
    public override void ResetWeaponAttackingPoiseBonus()
    {
        player.characterStatsManager.totalPoiseDefense = player.characterStatsManager.armorPoiseBonus;
    }
}


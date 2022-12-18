using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    PlayerManager player;
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
   
    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(weaponItem!=null)
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
            
            
        }
        
    }
}

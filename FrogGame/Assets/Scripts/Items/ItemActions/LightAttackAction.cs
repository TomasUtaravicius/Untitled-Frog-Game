using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
public class LightAttackAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if(character.characterStatsManager.currentStamina<=0)
        {
            return;
        }
        character.isAttacking = true;
        character.characterAnimatorHandler.EraseHandIKForWeapon();
        character.characterEffectsManager.PlayWeaponFX(false);
        if (character.isSprinting)
        {
            HandleRunningAttack(character);
            return;
        }
        if (character.canDoCombo)
        {
            HandleWeaponCombo(character);
        }
        else
        {
            if (character.isInteracting)
                return;
            if (character.canDoCombo)
                return;

            HandleLightAttack(character);
        }
        character.characterCombatManager.currentAttackType = AttackType.Light;
        
    }
    private void HandleLightAttack(CharacterManager character)
    {
        Debug.Log("Handle Light Attack");
        if(character.isUsingLeftHand)
        {
            character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_01, true, false, true);
            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
        }
        else if(character.isUsingRightHand)
        {
            if (character.isTwoHanding)
            {
                character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_light_attack_01, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_01;
            }
            else
            {

                character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_01, true);

                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
            }
        }
       

    }
    private void HandleRunningAttack(CharacterManager character)
    {
        Debug.Log("Handle Running Attack");
        if(character.isUsingLeftHand)
        {
            character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_running_attack_01, true, false, true);
            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_running_attack_01;
        }
        else if(character.isUsingRightHand)
        {
            if (character.isTwoHanding)
            {
                character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_running_attack_01, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.th_running_attack_01;
            }
            else
            {
                character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_running_attack_01, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_running_attack_01;
            }
        }
       
    }
    private void HandleWeaponCombo(CharacterManager character)
    {
        if (character.canDoCombo)
        {
            character.animator.SetBool("CanDoCombo", false);
            if(character.isUsingLeftHand)
            {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_01)
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_02;
                    }
                    else
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
                    }
            }
            else if(character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_light_attack_01)
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_light_attack_02, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_02;
                    }
                    else
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_light_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_01;
                    }
                }
                else
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_01)
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_02;
                    }
                    else
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
                    }
                }
            }
        }
    }
}

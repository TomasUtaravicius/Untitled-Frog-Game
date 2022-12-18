using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
public class ChargeAttackAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.characterStatsManager.currentStamina <= 0)
        {
            return;
        }
        character.characterAnimatorHandler.EraseHandIKForWeapon();
        character.characterEffectsManager.PlayWeaponFX(false);
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

            HandleChargeAttack(character);
        }

    }
    private void HandleChargeAttack(CharacterManager character)
    {
        if (character.isUsingLeftHand)
        {
            character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_charging_attack_01, true, false, true);
            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_01;
        }
        else if (character.isUsingRightHand)
        {
            if (character.isTwoHanding)
            {
                character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_charging_attack_01, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_01;
            }
            else
            {

                character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_charging_attack_01, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_01;
            }
        }
    }

    private void HandleWeaponCombo(CharacterManager character)
    {
        if (character.canDoCombo)
        {
            character.animator.SetBool("CanDoCombo", false);
            if (character.isUsingLeftHand)
            {
                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charging_attack_01)
                {
                    character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_charging_attack_02, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charging_attack_02;
                }
                else
                {
                    character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_charging_attack_02, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charging_attack_01;
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_charging_attack_01)
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_charging_attack_02, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_charging_attack_02;
                    }
                    else
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.th_charging_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_charging_attack_01;
                    }
                }
                else
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charging_attack_01)
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_charging_attack_02, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charging_attack_02;
                    }
                    else
                    {
                        character.characterAnimatorHandler.PlayTargetAnimation(character.characterCombatManager.oh_charging_attack_02, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charging_attack_01;
                    }
                }
            }
        }
    }
}

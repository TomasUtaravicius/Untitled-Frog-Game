using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
public class DrawArrowAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
            return;
        if (character.isHoldingArrow)
            return;
        character.animator.SetBool("IsHoldingArrow", true);
        character.characterAnimatorHandler.PlayTargetAnimation("Bow_TH_Draw_01", true);

        GameObject loadedArrow = Instantiate(character.characterInventoryManager.currentAmmo.loadedItemModel, character.characterWeaponSlotManager.leftHandSlot.transform);
        character.characterEffectsManager.currentRangeFX = loadedArrow;

        Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", true);
        bowAnimator.Play("Bow_Draw_01");
        
    }
}

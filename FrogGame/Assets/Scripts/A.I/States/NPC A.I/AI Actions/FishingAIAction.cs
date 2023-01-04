using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Actions/Fishing Action")]
public class FishingAIAction : AIAction
{
    public ToolItem fishingRod;
    public override void PerformAction(AIManager character)
    {
        if (character.isInteracting)
        {
            return;
        }
        character.characterWeaponSlotManager.LoadItemOnSlot(fishingRod, true);
        character.characterAnimatorHandler.PlayTargetAnimation("Fishing Cast", true, false);
        
    }
}

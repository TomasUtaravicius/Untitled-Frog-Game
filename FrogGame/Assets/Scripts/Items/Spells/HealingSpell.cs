using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(CharacterManager character)
    {
        base.AttemptToCastSpell(character);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFx, character.transform);
        character.characterAnimatorHandler.PlayTargetAnimation(spellAnimation, true,false, character.isUsingLeftHand);
        Debug.Log("Attempt to cast spell..");
    }
    public override void SuccessfullyCastSpell(CharacterManager character)
    {
        base.SuccessfullyCastSpell(character);
        GameObject instantiatedSpellFX = Instantiate(spellCastFx, character.transform);
        character.characterStatsManager.HealCharacter(healAmount);

        Debug.Log("Spell cast successful");
    }
}

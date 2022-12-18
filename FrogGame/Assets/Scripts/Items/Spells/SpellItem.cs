using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFx;
    public GameObject spellCastFx;

    [Header("Spell cost")]
    public int focusPointCost;

    public string spellAnimation;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(CharacterManager character)
    {
        Debug.Log("You attempt to cast a spell");
    }
    public virtual void SuccessfullyCastSpell(CharacterManager character)
    {
        PlayerManager player = character as PlayerManager;

        if(player!=null)
        {
            player.playerStatsManager.TakeFocusPointDrain(focusPointCost);
        }
        
    }
}

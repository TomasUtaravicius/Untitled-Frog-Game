using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public override void GrantWeaponAttackingPoiseBonus()
    {
        character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + character.characterStatsManager.offensivePoiseBonus;
    }
    public override void ResetWeaponAttackingPoiseBonus()
    {
        character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armorPoiseBonus;
    }

}

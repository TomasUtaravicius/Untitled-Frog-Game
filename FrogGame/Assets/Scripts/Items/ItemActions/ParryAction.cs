using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
            return;

        character.characterAnimatorHandler.EraseHandIKForWeapon();

        WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;

        if(parryingWeapon.weaponType == WeaponType.SmallShield)
        {
            character.characterAnimatorHandler.PlayTargetAnimation("Parry", true);
        }
        else if(parryingWeapon.weaponType == WeaponType.Shield)
        {
            character.characterAnimatorHandler.PlayTargetAnimation("Parry", true);
        }
    }
}

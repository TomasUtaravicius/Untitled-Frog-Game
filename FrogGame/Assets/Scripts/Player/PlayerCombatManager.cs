using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }
    public override void DrainStaminaBasedOnAttack()
    {
        if (player.isUsingRightHand)
        {
            if (currentAttackType == AttackType.Light)
            {
                player.playerStatsManager.DrainStamina(player.playerInventoryManager.rightWeapon.baseStamina * player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);
            }
            else if (currentAttackType == AttackType.Heavy)
            {
                player.playerStatsManager.DrainStamina(player.playerInventoryManager.rightWeapon.baseStamina * player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
            }
        }
        /*else if (player.isUsingLeftHand)
        {
            if (currentAttackType == AttackType.Light)
            {
                player.playerStatsManager.DrainStamina(player.playerInventoryManager.leftWeapon.baseStamina * player.playerInventoryManager.leftWeapon.lightAttackStaminaMultiplier);
            }
            else if (currentAttackType == AttackType.Heavy)
            {
                player.playerStatsManager.DrainStamina(player.playerInventoryManager.leftWeapon.baseStamina * player.playerInventoryManager.leftWeapon.heavyAttackStaminaMultiplier);
            }
        }*/
    }
}
